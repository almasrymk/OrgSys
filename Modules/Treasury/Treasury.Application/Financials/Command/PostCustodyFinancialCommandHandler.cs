namespace Treasury.Application.Financials.Commands;

using Accounting.Contracts.Accounts;
using Accounting.Contracts.Postings;
using MediatR;
using OrgSys.SharedKernel;
using System.Net;
using Treasury.Contracts.Financials;

public sealed class PostCustodyFinancialCommandHandler(
    IUnitOfWork unitOfWork,
    ISender sender,
    IRepository<Treasury.Domain.FinancialAccount> accountRepository,
    IRepository<FinancialType> typeRepository,
    IRepository<Treasury.Domain.Financial> transactionRepository)
    : ICommandHandler<PostCustodyFinancialCommand, long>
{
    public async Task<Result<long>> Handle(PostCustodyFinancialCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0 || request.ExchangeRate <= 0)
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Amount and exchange rate must be greater than zero.")]);

        var direction = request.Disbursement ? FinancialTransactionDirection.Out : FinancialTransactionDirection.In;
        var typeId = request.Disbursement ? FinancialTransactionType.Payment : FinancialTransactionType.Receipt;

        var account = await accountRepository.GetByFilterAsync(e => e.Id == request.FinancialAccountId, string.Empty);
        var type = await typeRepository.GetByFilterAsync(e => e.Id == (long)typeId, string.Empty);
        if (account is null || !account.IsActive || account.AccountId is not > 0 || type is null)
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Financial account and transaction type must be valid.")]);

        var counter = (await sender.Send(new GetAccountQuery(request.CounterAccountId), cancellationToken)).Response;
        if (counter is null)
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Counter account must be valid.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var now = DateTime.UtcNow;
            var transaction = new Treasury.Domain.Financial
            {
                FinancialAccountId = account.Id,
                FinancialTypeId = (long)typeId,
                PaymentTypeId = 1,
                Direction = direction,
                ReferenceType = FinancialReferenceType.Employee,
                ReferenceId = request.HolderId,
                ReferenceNumber = $"CUSTODY-{request.CustodyId}",
                Amount = request.Amount,
                AmountByDefaultCurrency = request.Amount * request.ExchangeRate,
                CurrencyId = request.CurrencyId,
                Rate = request.ExchangeRate,
                Date = request.TransactionDate,
                Notes = request.Description,
                CreateDate = now,
                CreateUserId = request.CreateUserId,
                BranchId = request.BranchId,
                ShiftId = request.ShiftId,
                Posted = true,
                Status = Status.Approved,
                TypeId = type.Id
            };
            await transactionRepository.CreateAsync(transaction);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            var financialGlId = account.AccountId!.Value;
            var debitId = direction == FinancialTransactionDirection.In ? financialGlId : counter.Id;
            var creditId = direction == FinancialTransactionDirection.In ? counter.Id : financialGlId;
            var note = request.Description ?? $"Custody {request.CustodyId}";

            var postResult = await sender.Send(new PostAccountingEntryCommand(
                ReferenceTable: "financialtransaction",
                SourceDocumentId: transaction.Id,
                SourceDocumentTypeId: type.Id,
                SourceDocumentCode: transaction.Code,
                JournalTypeId: 2,
                Date: request.TransactionDate,
                CreateDate: now,
                CreateUserId: request.CreateUserId,
                BranchId: request.BranchId,
                ShiftId: request.ShiftId,
                CurrencyId: request.CurrencyId,
                Rate: request.ExchangeRate,
                Note: note,
                Lines:
                [
                    new AccountingPostingLine(debitId, request.Amount, 0, note),
                    new AccountingPostingLine(creditId, 0, request.Amount, note)
                ]), cancellationToken);

            if (postResult.Response is null)
            {
                await unitOfWork.RollbackAsync();
                return new Result<long>(postResult.StatusCode, 0, postResult.Errors);
            }

            transaction.JournalId = postResult.Response.JournalId;
            transaction.HasJournal = true;
            await transactionRepository.UpdateAsync(transaction);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            await unitOfWork.CommitAsync();
            return new Result<long>(HttpStatusCode.OK, transaction.Id, null);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error(ex.Message)]);
        }
    }
}
