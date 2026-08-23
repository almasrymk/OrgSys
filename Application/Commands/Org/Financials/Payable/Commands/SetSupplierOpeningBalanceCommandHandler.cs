namespace Application.Commands.Org.Financials.Payable.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Services;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using System.Net;

    /// <summary>Records/updates a supplier's opening payable balance by adding (or updating) this
    /// supplier's line inside the single per-fiscal-year Opening Balance Journal that the existing
    /// manual Journal workflow already supports (<c>JournalType.IsOpeningBlance</c>, enforced by
    /// <see cref="IAccountingPeriodService.ValidateOpeningBalanceAsync"/>) — not a parallel per-supplier
    /// concept. A single aggregate "clearing" counter line is kept in balance automatically. Once that
    /// journal is Posted it is immutable, like every other Journal in this system — this command then
    /// refuses further edits; use the standard Journal Reverse workflow to correct a Posted opening
    /// balance. Mirrors <c>SetCustomerOpeningBalanceCommandHandler</c> on the AR side.</summary>
    public sealed record SetSupplierOpeningBalanceCommand(
        long DealerId,
        long FiscalYearId,
        decimal Amount,
        bool SupplierIsDebit,
        long CreateUserId) : ICommand, ICreateCommand<Result>;

    public sealed class SetSupplierOpeningBalanceCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Preference> _PreferenceRepository,
        IRepository<JournalType> _JournalTypeRepository,
        IRepository<Journal> _JournalRepository,
        IRepository<JournalItem> _JournalItemRepository,
        IRepository<Currency> _CurrencyRepository,
        IPayableAccountValidator _Validator,
        IReceivableAccountValidator _AccountValidator,
        IAccountingPeriodService _AccountingPeriodService) : ICommandHandler<SetSupplierOpeningBalanceCommand>
    {
        public async Task<Result> Handle(SetSupplierOpeningBalanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var (dealer, account, supplierErrors) = await _Validator.ValidateSupplierAsync(request.DealerId, cancellationToken);
            if (supplierErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, supplierErrors);

            var fiscalYear = await _AccountingPeriodService.GetFiscalYearAsync(request.FiscalYearId, cancellationToken);
            if (fiscalYear is null)
                return BadRequest("Fiscal year not found.");

            var openingJournalType = (await _JournalTypeRepository.GetListByFilterAsync(e => e.IsOpeningBlance))?.FirstOrDefault();
            if (openingJournalType is null)
                return BadRequest("No Opening Balance journal type is configured.");

            var preferences = (await _PreferenceRepository.GetListByFilterAsync(
                e => e.Reference == "Dealer" && e.TypeId == (long)DealerType.Supplier))?.ToList() ?? [];
            var clearingAccountId = long.TryParse(preferences.FirstOrDefault(e => e.Key == "OpeningBalanceClearingAccountId")?.Value, out var cid) ? cid : 0;
            if (clearingAccountId <= 0)
                return BadRequest("Opening balance clearing account is not configured (OpeningBalanceClearingAccountId preference).");

            var (clearingAccount, clearingErrors) = await _AccountValidator.ValidateAccountAsync(clearingAccountId, cancellationToken);
            if (clearingErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, clearingErrors);

            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var journal = await _JournalRepository.GetByFilterAsync(e =>
                    e.FiscalYearId == fiscalYear.Id &&
                    e.JournalTypeId == openingJournalType.Id &&
                    e.Status != Status.Deleted && e.Status != Status.Cancel, "JournalItems");

                if (journal is not null && journal.Posted)
                {
                    await _UnitOfWork.RollbackAsync();
                    return BadRequest("The Opening Balance journal for this fiscal year is already Posted and immutable. Use Reverse to correct it.");
                }

                AccountingPeriodResult? resolution = null;
                if (journal is null)
                {
                    resolution = await _AccountingPeriodService.ResolveAndValidateAsync(fiscalYear.StartDate, cancellationToken);
                    if (!resolution.Success)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    var obErrors = await _AccountingPeriodService.ValidateOpeningBalanceAsync(
                        openingJournalType.Id, fiscalYear.StartDate, fiscalYear, 0, cancellationToken);
                    if (obErrors.Count > 0)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, obErrors);
                    }

                    var currency = await _CurrencyRepository.GetByFilterAsync(e => e.IsDefault, string.Empty);
                    if (currency is null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return BadRequest("A default currency is required to create the Opening Balance journal.");
                    }

                    var codeNumber = await _JournalRepository.AnyAsync(e => e.TypeId == openingJournalType.Id, cancellationToken)
                        ? await _JournalRepository.GetMaxByFilterAsync(e => e.TypeId == openingJournalType.Id, e => e.CodeNumber) + 1 : 1;

                    journal = new Journal
                    {
                        JournalTypeId = openingJournalType.Id,
                        TypeId = openingJournalType.Id,
                        CodeNumber = codeNumber,
                        Code = codeNumber.ToString(),
                        Date = fiscalYear.StartDate,
                        CreateDate = DateTime.Now,
                        CreateUserId = request.CreateUserId,
                        CurrencyId = currency.Id,
                        Rate = currency.Rate,
                        FiscalYearId = fiscalYear.Id,
                        FiscalPeriodId = resolution.FiscalPeriod!.Id,
                        Note = "Supplier Opening Balances"
                    };
                    await _JournalRepository.CreateAsync(journal);
                    await _UnitOfWork.SaveChangeAsync(cancellationToken);
                }

                var debit = request.SupplierIsDebit ? request.Amount : 0;
                var credit = request.SupplierIsDebit ? 0 : request.Amount;
                var existingLine = journal.JournalItems?.FirstOrDefault(e => e.AccountId == account!.Id);

                if (existingLine is not null)
                {
                    existingLine.Debit = debit;
                    existingLine.Credit = credit;
                    existingLine.Note = $"Opening balance — {dealer!.Name}";
                    await _JournalItemRepository.UpdateAsync(existingLine);
                }
                else
                {
                    await _JournalItemRepository.CreateAsync(new JournalItem
                    {
                        JournalId = journal.Id,
                        AccountId = account!.Id,
                        Debit = debit,
                        Credit = credit,
                        Note = $"Opening balance — {dealer!.Name}"
                    });
                }
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                // Rebalance the single aggregate clearing line against everything else in this journal.
                var allLines = (await _JournalItemRepository.GetListByFilterAsync(e => e.JournalId == journal.Id))?.ToList() ?? [];
                var otherLines = allLines.Where(e => e.AccountId != clearingAccount!.Id).ToList();
                var net = otherLines.Sum(e => e.Debit) - otherLines.Sum(e => e.Credit);
                var clearingLine = allLines.FirstOrDefault(e => e.AccountId == clearingAccount!.Id);
                var clearingDebit = net < 0 ? -net : 0;
                var clearingCredit = net > 0 ? net : 0;

                if (clearingLine is not null)
                {
                    clearingLine.Debit = clearingDebit;
                    clearingLine.Credit = clearingCredit;
                    await _JournalItemRepository.UpdateAsync(clearingLine);
                }
                else if (clearingDebit != 0 || clearingCredit != 0)
                {
                    await _JournalItemRepository.CreateAsync(new JournalItem
                    {
                        JournalId = journal.Id,
                        AccountId = clearingAccount!.Id,
                        Debit = clearingDebit,
                        Credit = clearingCredit,
                        Note = "Opening balance clearing"
                    });
                }

                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                }

                await _UnitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }

        private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
    }
}
