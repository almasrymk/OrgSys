namespace Application.Commands.Org.Financials.Financial.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Services;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using System.Net;

    public sealed record PostFinancialOpeningBalanceCommand(long Id, long UserId) : ICommand, IUpdateCommand<Result>;

    /// <summary>
    /// Posts a Draft Opening Balance <see cref="Financial"/> row (FinancialTypeId = OpeningBalance):
    /// creates its own Journal — Dr the FinancialAccount's linked GL account, Cr the Preferences-configured
    /// OpeningBalanceEquityAccountId — same JournalTypeId=2 "General" shape PostTransactionCommandHandler
    /// uses for Receipt/Payment/etc, so it stays outside the separate shared per-fiscal-year Opening
    /// Balance Journal that Customer/Supplier (and previously FinancialAccount) opening balances use.
    /// </summary>
    public sealed class PostFinancialOpeningBalanceCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Financial> financialRepository,
        IRepository<FinancialAccount> accountRepository,
        IRepository<Preference> preferenceRepository,
        IRepository<Journal> journalRepository,
        IAccountingPeriodService accountingPeriodService) : ICommandHandler<PostFinancialOpeningBalanceCommand>
    {
        public async Task<Result> Handle(PostFinancialOpeningBalanceCommand request, CancellationToken cancellationToken)
        {
            var financial = await financialRepository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (financial is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Opening Balance record not found.")]);

            if (financial.FinancialTypeId != (long)FinancialTransactionType.OpeningBalance)
                return new Result(HttpStatusCode.BadRequest, [new Error("Only an Opening Balance record can be posted through this action.")]);

            if (financial.Posted)
                return new Result(HttpStatusCode.BadRequest, [new Error("This Opening Balance has already been posted.")]);

            if (financial.Status == Status.Cancel || financial.Status == Status.Deleted)
                return new Result(HttpStatusCode.BadRequest, [new Error("A cancelled or deleted Opening Balance cannot be posted.")]);

            if (financial.Amount <= 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Opening Balance amount must be greater than zero.")]);

            if (financial.FinancialAccountId is not > 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Financial account is required.")]);

            var account = await accountRepository.GetByFilterAsync(e => e.Id == financial.FinancialAccountId, "");
            if (account is null || !account.IsActive)
                return new Result(HttpStatusCode.BadRequest, [new Error("Financial account is invalid or inactive.")]);

            var preferences = (await preferenceRepository.GetListByFilterAsync(
                e => e.Reference == "Financial" && e.TypeId == (long)FinancialTransactionType.OpeningBalance))?.ToList() ?? [];

            // Accounts Integration on: Debit the preference-configured Cash Box/Bank GL account (by the
            // FinancialAccount's own type) instead of requiring each Cash Box/Bank record to carry its
            // own linked GL account — same "default account per scenario" idea Invoice/Transaction
            // preferences already use. Off (or unconfigured): fall back to the FinancialAccount's own
            // AccountId, the original behavior.
            long debitAccountId;
            if (preferences.FirstOrDefault(e => e.Key == "AccountsIntegration")?.Value == "1")
            {
                var isBank = account.FinancialAccountType == FinancialAccountType.Bank;
                var integrationAccountRaw = preferences.FirstOrDefault(e => e.Key == (isBank ? "BankAccount" : "CashBoxAccount"))?.Value;
                if (!long.TryParse(integrationAccountRaw, out debitAccountId) || debitAccountId <= 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error(
                        $"Accounts Integration is enabled but the {(isBank ? "Bank" : "Cash Box")} Account is not configured (Preferences > Financial > Opening Balance).")]);
            }
            else
            {
                if (account.AccountId is not > 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Financial account must be linked to a general-ledger account.")]);
                debitAccountId = account.AccountId.Value;
            }

            var equityAccountIdRaw = preferences.FirstOrDefault(e => e.Key == "OpeningBalanceEquityAccountId")?.Value;
            if (!long.TryParse(equityAccountIdRaw, out var equityAccountId) || equityAccountId <= 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Opening Balance equity account is not configured (Preferences > Financial > Opening Balance Equity Account).")]);

            var resolution = await accountingPeriodService.ResolveAndValidateAsync(financial.Date, cancellationToken);
            if (!resolution.Success)
                return new Result(HttpStatusCode.BadRequest, resolution.Errors);

            // Re-checked here too (not just at Draft-save time) in case two Drafts were saved for the
            // same account/year before either was posted — only one may ever end up Posted.
            var duplicate = await financialRepository.AnyAsync(e =>
                e.Id != financial.Id &&
                e.FinancialTypeId == (long)FinancialTransactionType.OpeningBalance &&
                e.FinancialAccountId == financial.FinancialAccountId &&
                e.Posted &&
                e.Status != Status.Deleted && e.Status != Status.Reversed && e.Hide != true &&
                e.Date.Date >= resolution.FiscalYear!.StartDate.Date && e.Date.Date <= resolution.FiscalYear.EndDate.Date,
                cancellationToken);
            if (duplicate)
                return new Result(HttpStatusCode.BadRequest, [new Error("An Opening Balance has already been posted for this Financial Account in this fiscal year.")]);

            await unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;
                var codeNumber = await journalRepository.AnyAsync(e => e.TypeId == 2, cancellationToken)
                    ? await journalRepository.GetMaxByFilterAsync(e => e.TypeId == 2, e => e.CodeNumber) + 1 : 1;

                var journal = new Journal
                {
                    JournalTypeId = 2,
                    TypeId = 2,
                    CodeNumber = codeNumber,
                    Code = codeNumber.ToString(),
                    Date = financial.Date,
                    CreateDate = now,
                    CreateUserId = request.UserId,
                    BranchId = financial.BranchId,
                    ShiftId = financial.ShiftId,
                    CurrencyId = financial.CurrencyId,
                    Rate = financial.Rate,
                    RefranceId = financial.Id,
                    RefranceCode = financial.Code,
                    RefranceTypeId = (long)FinancialTransactionType.OpeningBalance,
                    RefranceTable = "financialtransaction",
                    Note = financial.Notes,
                    FiscalYearId = resolution.FiscalYear!.Id,
                    FiscalPeriodId = resolution.FiscalPeriod!.Id,
                    Posted = true,
                    Status = Status.Approved,
                    JournalItems =
                    [
                        new JournalItem { AccountId = debitAccountId, Debit = financial.Amount, Note = financial.Notes },
                        new JournalItem { AccountId = equityAccountId, Credit = financial.Amount, Note = financial.Notes }
                    ]
                };
                await journalRepository.CreateAsync(journal);
                await unitOfWork.SaveChangeAsync(cancellationToken);

                financial.JournalId = journal.Id;
                financial.HasJournal = true;
                financial.Posted = true;
                financial.Status = Status.Approved;
                financial.ModifyUserId = request.UserId;
                financial.ModifyDate = now;
                await financialRepository.UpdateAsync(financial);

                if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                }

                await unitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
