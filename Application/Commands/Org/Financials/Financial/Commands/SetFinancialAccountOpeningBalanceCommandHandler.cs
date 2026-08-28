namespace Application.Commands.Org.Financials.Financial.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Services;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using System.Net;

    /// <summary>Records/updates a FinancialAccount's (Cash Box/Bank) opening balance line inside the SAME
    /// single per-fiscal-year Opening Balance Journal that
    /// <see cref="Application.Commands.Org.Financials.Receivable.Commands.SetCustomerOpeningBalanceCommandHandler"/>
    /// and its Supplier counterpart already use (JournalType.IsOpeningBlance, enforced by
    /// IAccountingPeriodService.ValidateOpeningBalanceAsync — only one such journal may exist per fiscal
    /// year, company-wide). Rebalances only the lines this handler owns — FinancialAccount-linked GL
    /// accounts plus its own equity/clearing account (Preference "OpeningBalanceEquityAccountId" under the
    /// Financial/OpeningBalance resource) — so it never touches the Dealer clearing line the Customer/
    /// Supplier flow manages independently inside the same journal. Once that journal is Posted it is
    /// immutable, like every other Journal in this system — this command then refuses further edits; use
    /// the standard Journal Reverse workflow (Journal/Save screen) to correct a Posted opening balance.</summary>
    public sealed record SetFinancialAccountOpeningBalanceCommand(
        long FinancialAccountId,
        long FiscalYearId,
        decimal Debit,
        decimal Credit,
        long? CurrencyId,
        decimal Rate,
        string? Notes,
        long CreateUserId) : ICommand<long>;

    public sealed class SetFinancialAccountOpeningBalanceCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.FinancialAccount> _FinancialAccountRepository,
        IRepository<Preference> _PreferenceRepository,
        IRepository<JournalType> _JournalTypeRepository,
        IRepository<Journal> _JournalRepository,
        IRepository<JournalItem> _JournalItemRepository,
        IRepository<Currency> _CurrencyRepository,
        IAccountingPeriodService _AccountingPeriodService) : ICommandHandler<SetFinancialAccountOpeningBalanceCommand, long>
    {
        public async Task<Result<long>> Handle(SetFinancialAccountOpeningBalanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Debit < 0 || request.Credit < 0)
                return BadRequest("Debit and Credit must not be negative.");
            if (request.Debit > 0 && request.Credit > 0)
                return BadRequest("Enter either Debit or Credit, not both.");
            if (request.Debit == 0 && request.Credit == 0)
                return BadRequest("Enter a Debit or Credit amount greater than zero.");

            var financialAccount = await _FinancialAccountRepository.GetByFilterAsync(e => e.Id == request.FinancialAccountId, string.Empty);
            if (financialAccount is null || !financialAccount.IsActive || financialAccount.AccountId is not > 0)
                return BadRequest("Financial account is invalid or has no linked GL account.");

            var fiscalYear = await _AccountingPeriodService.GetFiscalYearAsync(request.FiscalYearId, cancellationToken);
            if (fiscalYear is null)
                return BadRequest("Fiscal year not found.");

            var openingJournalType = (await _JournalTypeRepository.GetListByFilterAsync(e => e.IsOpeningBlance))?.FirstOrDefault();
            if (openingJournalType is null)
                return BadRequest("No Opening Balance journal type is configured.");

            var preferences = (await _PreferenceRepository.GetListByFilterAsync(
                e => e.Reference == "Financial" && e.TypeId == 1))?.ToList() ?? [];
            var equityAccountId = long.TryParse(preferences.FirstOrDefault(e => e.Key == "OpeningBalanceEquityAccountId")?.Value, out var eid) ? eid : 0;
            if (equityAccountId <= 0)
                return BadRequest("Opening balance equity account is not configured (OpeningBalanceEquityAccountId preference).");

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

                if (journal is null)
                {
                    var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(fiscalYear.StartDate, cancellationToken);
                    if (!resolution.Success)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result<long>(HttpStatusCode.BadRequest, default, resolution.Errors);
                    }

                    var obErrors = await _AccountingPeriodService.ValidateOpeningBalanceAsync(
                        openingJournalType.Id, fiscalYear.StartDate, fiscalYear, 0, cancellationToken);
                    if (obErrors.Count > 0)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result<long>(HttpStatusCode.BadRequest, default, obErrors);
                    }

                    var currency = request.CurrencyId is > 0
                        ? await _CurrencyRepository.GetByFilterAsync(e => e.Id == request.CurrencyId, string.Empty)
                        : await _CurrencyRepository.GetByFilterAsync(e => e.IsDefault, string.Empty);
                    if (currency is null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return BadRequest("A currency is required to create the Opening Balance journal.");
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
                        Rate = request.Rate > 0 ? request.Rate : currency.Rate,
                        FiscalYearId = fiscalYear.Id,
                        FiscalPeriodId = resolution.FiscalPeriod!.Id,
                        Note = "Opening Balances"
                    };
                    await _JournalRepository.CreateAsync(journal);
                    await _UnitOfWork.SaveChangeAsync(cancellationToken);
                }

                var financialAccountGlId = financialAccount.AccountId!.Value;
                var existingLine = journal.JournalItems?.FirstOrDefault(e => e.AccountId == financialAccountGlId);
                var note = string.IsNullOrWhiteSpace(request.Notes) ? $"Opening balance — {financialAccount.Name}" : request.Notes;

                if (existingLine is not null)
                {
                    existingLine.Debit = request.Debit;
                    existingLine.Credit = request.Credit;
                    existingLine.Note = note;
                    await _JournalItemRepository.UpdateAsync(existingLine);
                }
                else
                {
                    await _JournalItemRepository.CreateAsync(new JournalItem
                    {
                        JournalId = journal.Id,
                        AccountId = financialAccountGlId,
                        Debit = request.Debit,
                        Credit = request.Credit,
                        Note = note
                    });
                }
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                // Rebalance only the lines this handler owns — every FinancialAccount-linked GL account plus
                // its own equity line — never the Dealer flow's own lines/clearing account in this same journal.
                var financialAccountGlIds = ((await _FinancialAccountRepository.GetListByFilterAsync(e => e.AccountId != null))
                    ?.Select(e => e.AccountId!.Value).ToHashSet()) ?? [];
                financialAccountGlIds.Add(equityAccountId);

                var allLines = (await _JournalItemRepository.GetListByFilterAsync(e => e.JournalId == journal.Id))?.ToList() ?? [];
                var ownedLines = allLines.Where(e => financialAccountGlIds.Contains(e.AccountId)).ToList();
                var otherOwnedLines = ownedLines.Where(e => e.AccountId != equityAccountId).ToList();
                var net = otherOwnedLines.Sum(e => e.Debit) - otherOwnedLines.Sum(e => e.Credit);
                var equityLine = ownedLines.FirstOrDefault(e => e.AccountId == equityAccountId);
                var equityDebit = net < 0 ? -net : 0;
                var equityCredit = net > 0 ? net : 0;

                if (equityLine is not null)
                {
                    equityLine.Debit = equityDebit;
                    equityLine.Credit = equityCredit;
                    await _JournalItemRepository.UpdateAsync(equityLine);
                }
                else if (equityDebit != 0 || equityCredit != 0)
                {
                    await _JournalItemRepository.CreateAsync(new JournalItem
                    {
                        JournalId = journal.Id,
                        AccountId = equityAccountId,
                        Debit = equityDebit,
                        Credit = equityCredit,
                        Note = "Opening balance equity"
                    });
                }

                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result<long>(HttpStatusCode.InternalServerError, default, [new Error("Error saving changes")]);
                }

                await _UnitOfWork.CommitAsync();
                return new Result<long>(HttpStatusCode.OK, journal.Id, null);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result<long>(HttpStatusCode.InternalServerError, default, [new Error(ex.Message)]);
            }
        }

        private static Result<long> BadRequest(string message) => new(HttpStatusCode.BadRequest, default, [new Error(message)]);
    }
}
