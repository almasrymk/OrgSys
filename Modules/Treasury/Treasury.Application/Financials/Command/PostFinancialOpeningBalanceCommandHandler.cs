namespace Treasury.Application.Financials.Commands
{
    using Accounting.Contracts.Postings;
    using Administration.Contracts.Preferences;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record PostFinancialOpeningBalanceCommand(long Id, long UserId) : ICommand, IUpdateCommand<Result>;

    /// <summary>
    /// Posts a Draft Opening Balance <see cref="Financial"/> row (FinancialTypeId = OpeningBalance):
    /// creates its own Journal — Dr the FinancialAccount's linked GL account, Cr the Preferences-configured
    /// OpeningBalanceEquityAccountId — same JournalTypeId=2 "General" shape PostTransactionCommandHandler
    /// uses for Receipt/Payment/etc, so it stays outside the separate shared per-fiscal-year Opening
    /// Balance Journal that Customer/Supplier (and previously FinancialAccount) opening balances use.
    /// Journal creation/posting itself is delegated to Accounting.Contracts.Postings.
    /// PostAccountingEntryCommand — this handler only resolves which accounts/amounts apply.
    /// </summary>
    public sealed class PostFinancialOpeningBalanceCommandHandler(
        IUnitOfWork unitOfWork,
        ISender sender,
        IRepository<Financial> financialRepository,
        IRepository<FinancialAccount> accountRepository) : ICommandHandler<PostFinancialOpeningBalanceCommand>
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

            var preferences = (await sender.Send(
                new GetPreferenceValuesQuery("Financial", (long)FinancialTransactionType.OpeningBalance),
                cancellationToken)).Response ?? new Dictionary<string, string?>();

            // Accounts Integration on: Debit the preference-configured Cash Box/Bank GL account (by the
            // FinancialAccount's own type) instead of requiring each Cash Box/Bank record to carry its
            // own linked GL account — same "default account per scenario" idea Invoice/Transaction
            // preferences already use. Off (or unconfigured): fall back to the FinancialAccount's own
            // AccountId, the original behavior.
            long debitAccountId;
            if (preferences.TryGetValue("AccountsIntegration", out var accountsIntegration) && accountsIntegration == "1")
            {
                var isBank = account.FinancialAccountType == FinancialAccountType.Bank;
                preferences.TryGetValue(isBank ? "BankAccount" : "CashBoxAccount", out var integrationAccountRaw);
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

            preferences.TryGetValue("OpeningBalanceEquityAccountId", out var equityAccountIdRaw);
            if (!long.TryParse(equityAccountIdRaw, out var equityAccountId) || equityAccountId <= 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("Opening Balance equity account is not configured (Preferences > Financial > Opening Balance Equity Account).")]);

            var fiscalYear = (await sender.Send(new GetFiscalYearForDateQuery(financial.Date), cancellationToken)).Response;
            if (fiscalYear is null)
                return new Result(HttpStatusCode.BadRequest, [new Error($"No fiscal year is configured for date {financial.Date:yyyy-MM-dd}.")]);

            // Re-checked here too (not just at Draft-save time) in case two Drafts were saved for the
            // same account/year before either was posted — only one may ever end up Posted.
            var duplicate = await financialRepository.AnyAsync(e =>
                e.Id != financial.Id &&
                e.FinancialTypeId == (long)FinancialTransactionType.OpeningBalance &&
                e.FinancialAccountId == financial.FinancialAccountId &&
                e.Posted &&
                e.Status != Status.Deleted && e.Status != Status.Reversed && e.Hide != true &&
                e.Date.Date >= fiscalYear.StartDate.Date && e.Date.Date <= fiscalYear.EndDate.Date,
                cancellationToken);
            if (duplicate)
                return new Result(HttpStatusCode.BadRequest, [new Error("An Opening Balance has already been posted for this Financial Account in this fiscal year.")]);

            await unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;

                var postResult = await sender.Send(new PostAccountingEntryCommand(
                    ReferenceTable: "financialtransaction",
                    SourceDocumentId: financial.Id,
                    SourceDocumentTypeId: (long)FinancialTransactionType.OpeningBalance,
                    SourceDocumentCode: financial.Code,
                    JournalTypeId: 2,
                    Date: financial.Date,
                    CreateDate: now,
                    CreateUserId: request.UserId,
                    BranchId: financial.BranchId,
                    ShiftId: financial.ShiftId,
                    CurrencyId: financial.CurrencyId,
                    Rate: financial.Rate,
                    Note: financial.Notes,
                    Lines:
                    [
                        new AccountingPostingLine(debitAccountId, financial.Amount, 0, financial.Notes),
                        new AccountingPostingLine(equityAccountId, 0, financial.Amount, financial.Notes)
                    ]), cancellationToken);

                if (postResult.Response is null)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result(postResult.StatusCode, postResult.Errors);
                }

                financial.JournalId = postResult.Response.JournalId;
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
