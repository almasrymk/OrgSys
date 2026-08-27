namespace Application.Commands.Org.Financials.Financial.Commands
{
    using Application.Abstraction.Command;
    using Application.DTOs;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using System.Net;

    public sealed record PostFinancialTransactionCommand(PostFinancialTransactionDto Transaction) : ICommand, ICreateCommand<Result>;

    public sealed class PostTransactionCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Domain.Entities.FinancialAccount> accountRepository,
        IRepository<FinancialType> typeRepository,
        IRepository<Account> glRepository,
        IRepository<Domain.Entities.Financial> transactionRepository,
        IRepository<Journal> journalRepository,
        Application.Common.Services.IAccountingPeriodService accountingPeriodService) : ICommandHandler<PostFinancialTransactionCommand>
    {
        public async Task<Result> Handle(PostFinancialTransactionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Transaction;
            if (dto.Amount <= 0 || dto.ExchangeRate <= 0)
                return BadRequest("Amount and exchange rate must be greater than zero.");
            var account = await accountRepository.GetByFilterAsync(e => e.Id == dto.FinancialAccountId, string.Empty);
            var type = await typeRepository.GetByFilterAsync(e => e.Id == dto.FinancialTypeId, string.Empty);
            var counter = await glRepository.GetByFilterAsync(e => e.Id == dto.CounterAccountId, string.Empty);
            if (account is null || !account.IsActive || account.AccountId is not > 0 || type is null || counter is null)
                return BadRequest("Financial account, transaction type, and counter account must be valid.");

            var resolution = await accountingPeriodService.ResolveAndValidateAsync(dto.TransactionDate, cancellationToken);
            if (!resolution.Success)
                return new Result(HttpStatusCode.BadRequest, resolution.Errors);

            await unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.UtcNow;
                var transaction = new Domain.Entities.Financial
                {
                    FinancialAccountId = account.Id,
                    FinancialTypeId = type.Id,
                    FinancialTransactionType = dto.FinancialTransactionType,
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

        private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
    }
}
