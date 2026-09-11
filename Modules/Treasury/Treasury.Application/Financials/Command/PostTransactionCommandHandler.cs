namespace Treasury.Application.Financials.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record PostFinancialTransactionCommand(PostFinancialTransactionDto Transaction) : ICommand, ICreateCommand<Result>;

    public sealed class PostTransactionCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Treasury.Domain.FinancialAccount> accountRepository,
        IRepository<FinancialType> typeRepository,
        IRepository<Account> glRepository,
        IRepository<Treasury.Domain.Financial> transactionRepository,
        IRepository<Sales.Domain.Invoice> invoiceRepository,
        IRepository<Journal> journalRepository,
        Accounting.Application.IReceivableAccountValidator referenceValidator,
        Accounting.Application.IAccountingPeriodService accountingPeriodService) : ICommandHandler<PostFinancialTransactionCommand>
    {
        public async Task<Result> Handle(PostFinancialTransactionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Transaction;
            if (dto.Amount <= 0 || dto.ExchangeRate <= 0)
                return BadRequest("Amount and exchange rate must be greater than zero.");
            var account = await accountRepository.GetByFilterAsync(e => e.Id == dto.FinancialAccountId, string.Empty);
            var type = await typeRepository.GetByFilterAsync(e => e.Id == (long)dto.FinancialTypeId, string.Empty);
            if (account is null || !account.IsActive || account.AccountId is not > 0 || type is null)
                return BadRequest("Financial account and transaction type must be valid.");

            // Receipt/Payment only: the Counter GL Account is driven by the selected Reference (Customer/
            // Supplier/Expense/Income Account) instead of being freely typed in — re-resolve it here from
            // the reference's own linked account rather than trusting whatever CounterAccountId the client
            // posted, and verify the reference actually exists in its real table (not just a non-empty Id).
            // Invoice/Payment/Transfer references have no linked account (by design, see the DTOs' own
            // comments) so CounterAccountId still comes from the client for those; Employee/Loan/Cheque/
            // PaymentGateway have no backing table in this codebase yet, so they're left unvalidated.
            if (dto.FinancialTypeId is FinancialTransactionType.Receipt or FinancialTransactionType.Payment)
            {
                var referenceError = await ResolveReference(dto, cancellationToken);
                if (referenceError is not null)
                    return BadRequest(referenceError);
            }

            var counter = await glRepository.GetByFilterAsync(e => e.Id == dto.CounterAccountId, string.Empty);
            if (counter is null)
                return BadRequest("Counter account must be valid.");

            var resolution = await accountingPeriodService.ResolveAndValidateAsync(dto.TransactionDate, cancellationToken);
            if (!resolution.Success)
                return new Result(HttpStatusCode.BadRequest, resolution.Errors);

            await unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.UtcNow;
                var transaction = new Treasury.Domain.Financial
                {
                    FinancialAccountId = account.Id,
                    FinancialTypeId = (long)dto.FinancialTypeId,
                    // Financial.PaymentTypeId is a required FK with no equivalent field on
                    // PostFinancialTransactionDto — default to Cash (Id=1), the same convention
                    // already used for Opening Balance (see PostFinancialOpeningBalanceCommandHandler).
                    PaymentTypeId = 1,
                    Direction = dto.Direction,
                    ReferenceType = dto.ReferenceType,
                    ReferenceId = dto.ReferenceId,
                    ReferenceNumber = dto.ReferenceNumber,
                    DealerId = dto.DealerId,
                    Amount = dto.Amount,
                    AmountByDefaultCurrency = dto.Amount * dto.ExchangeRate,
                    CurrencyId = dto.CurrencyId,
                    Rate = dto.ExchangeRate,
                    Date = dto.TransactionDate,
                    Notes = dto.Description,
                    CreateDate = now,
                    CreateUserId = dto.CreateUserId,
                    BranchId = dto.BranchId,
                    ShiftId = dto.ShiftId,
                    Posted = true,
                    Status = Status.Approved,
                    TypeId = type.Id
                };
                await transactionRepository.CreateAsync(transaction);
                await unitOfWork.SaveChangeAsync(cancellationToken);

                var codeNumber = await journalRepository.AnyAsync(e => e.TypeId == 2, cancellationToken)
                    ? await journalRepository.GetMaxByFilterAsync(e => e.TypeId == 2, e => e.CodeNumber) + 1 : 1;
                var financialGlId = account.AccountId.Value;
                var debitId = dto.Direction == FinancialTransactionDirection.In ? financialGlId : counter.Id;
                var creditId = dto.Direction == FinancialTransactionDirection.In ? counter.Id : financialGlId;
                var journal = new Journal
                {
                    JournalTypeId = 2, TypeId = 2, CodeNumber = codeNumber, Code = codeNumber.ToString(),
                    Date = dto.TransactionDate, CreateDate = now, CreateUserId = dto.CreateUserId,
                    BranchId = dto.BranchId, ShiftId = dto.ShiftId, CurrencyId = dto.CurrencyId,
                    Rate = dto.ExchangeRate, RefranceId = transaction.Id, RefranceCode = transaction.Code,
                    RefranceTypeId = type.Id, RefranceTable = "financialtransaction", Note = dto.Description,
                    FiscalYearId = resolution.FiscalYear!.Id, FiscalPeriodId = resolution.FiscalPeriod!.Id,
                    Posted = true, Status = Status.Approved,
                    JournalItems =
                    [
                        new JournalItem { AccountId = debitId, Debit = dto.Amount, Note = dto.Description },
                        new JournalItem { AccountId = creditId, Credit = dto.Amount, Note = dto.Description }
                    ]
                };
                await journalRepository.CreateAsync(journal);
                await unitOfWork.SaveChangeAsync(cancellationToken);
                transaction.JournalId = journal.Id;
                transaction.HasJournal = true;
                await transactionRepository.UpdateAsync(transaction);
                await unitOfWork.SaveChangeAsync(cancellationToken);
                await unitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }

        // Resolves and validates dto.ReferenceId against its real backing table for the given
        // ReferenceType, overriding dto.CounterAccountId with the reference's own linked account where
        // the domain defines one (Customer/Supplier/Expense/Income). Returns an error message, or null
        // on success. Employee/Loan/Cheque/PaymentGateway have no backing entity in this codebase yet, so
        // they fall through unvalidated — the UI never offers them a reference to pick in the first place.
        private async Task<string?> ResolveReference(PostFinancialTransactionDto dto, CancellationToken cancellationToken)
        {
            switch (dto.ReferenceType)
            {
                case FinancialReferenceType.Other:
                    return null;

                case FinancialReferenceType.Customer:
                {
                    if (dto.ReferenceId is not > 0)
                        return "A customer reference is required.";
                    var (_, refAccount, errors) = await referenceValidator.ValidateCustomerAsync(dto.ReferenceId.Value, cancellationToken);
                    if (errors.Count > 0)
                        return string.Join(" ", errors.Select(e => e.MessageError));
                    dto.CounterAccountId = refAccount!.Id;
                    return null;
                }

                case FinancialReferenceType.Supplier:
                {
                    if (dto.ReferenceId is not > 0)
                        return "A supplier reference is required.";
                    var (_, refAccount, errors) = await referenceValidator.ValidateSupplierAsync(dto.ReferenceId.Value, cancellationToken);
                    if (errors.Count > 0)
                        return string.Join(" ", errors.Select(e => e.MessageError));
                    dto.CounterAccountId = refAccount!.Id;
                    return null;
                }

                case FinancialReferenceType.Expense:
                case FinancialReferenceType.Income:
                {
                    var isExpense = dto.ReferenceType == FinancialReferenceType.Expense;
                    if (dto.ReferenceId is not > 0)
                        return isExpense ? "An expense account reference is required." : "An income account reference is required.";

                    // Needs AccountType.Name for the Expense/Revenue classification check — the shared
                    // validator doesn't load that nav, so this queries directly instead of via ValidateAccountAsync.
                    var refAccount = await glRepository.GetByFilterAsync(e => e.Id == dto.ReferenceId, "AccountType");
                    if (refAccount is null || refAccount.Status == Status.Deleted || refAccount.Hide)
                        return "The selected account does not exist or is not active.";
                    if (!refAccount.IsPostable)
                        return $"Account '{refAccount.Name}' is a parent/group account and cannot receive postings.";

                    var expectedTypeName = isExpense ? "Expense" : "Revenue";
                    if (!string.Equals(refAccount.AccountType?.Name, expectedTypeName, StringComparison.OrdinalIgnoreCase))
                        return isExpense
                            ? $"Account '{refAccount.Name}' is not classified as an expense account."
                            : $"Account '{refAccount.Name}' is not classified as a revenue account.";

                    dto.CounterAccountId = refAccount.Id;
                    return null;
                }

                case FinancialReferenceType.Invoice:
                {
                    if (dto.ReferenceId is not > 0)
                        return "An invoice reference is required.";
                    var invoice = await invoiceRepository.GetByFilterAsync(e => e.Id == dto.ReferenceId, string.Empty);
                    if (invoice is null || invoice.Status == Status.Deleted || invoice.Hide)
                        return "The selected invoice does not exist.";
                    // No linked GL account on an invoice reference — CounterAccountId stays client-supplied.
                    return null;
                }

                case FinancialReferenceType.Payment:
                {
                    if (dto.ReferenceId is not > 0)
                        return "A payment reference is required.";
                    var exists = await transactionRepository.AnyAsync(e =>
                        e.Id == dto.ReferenceId && e.FinancialTypeId == (long)FinancialTransactionType.Payment, cancellationToken);
                    if (!exists)
                        return "The selected payment reference does not exist.";
                    return null;
                }

                case FinancialReferenceType.Transfer:
                {
                    if (dto.ReferenceId is not > 0)
                        return "A transfer reference is required.";
                    var exists = await transactionRepository.AnyAsync(e =>
                        e.Id == dto.ReferenceId
                        && (e.FinancialTypeId == (long)FinancialTransactionType.TransferIn || e.FinancialTypeId == (long)FinancialTransactionType.TransferOut),
                        cancellationToken);
                    if (!exists)
                        return "The selected transfer reference does not exist.";
                    return null;
                }

                default:
                    return null;
            }
        }

        private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
    }
}
