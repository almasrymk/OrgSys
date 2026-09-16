namespace Advances.Application.Custodies.Commands;

using Advances.Domain.Exceptions;
using MediatR;
using System.Net;
using Treasury.Contracts.Financials;

public sealed record ReturnCustodyCommand(
    long Id,
    decimal Amount,
    long FinancialAccountId,
    long CounterAccountId,
    DateTime ReturnDate,
    long CreateUserId,
    long? BranchId,
    long? ShiftId) : ICommand;

public sealed class ReturnCustodyCommandHandler(
    IRepository<Custody> repository,
    IUnitOfWork unitOfWork,
    ISender sender)
    : ICommandHandler<ReturnCustodyCommand>
{
    public async Task<Result> Handle(ReturnCustodyCommand request, CancellationToken cancellationToken)
    {
        var custody = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, string.Empty);
        if (custody is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Custody not found.")]);

        if (custody.LifecycleStatus is not (CustodyStatus.Issued or CustodyStatus.PartiallySettled))
            return new Result(HttpStatusCode.BadRequest, [new Error($"Cannot return an amount on a custody that is {custody.LifecycleStatus}.")]);

        var posted = await sender.Send(new PostCustodyFinancialCommand(
            Disbursement: false,
            FinancialAccountId: request.FinancialAccountId,
            CounterAccountId: request.CounterAccountId,
            Amount: request.Amount,
            CurrencyId: custody.CurrencyId,
            ExchangeRate: custody.Rate,
            TransactionDate: request.ReturnDate == default ? DateTime.Now : request.ReturnDate,
            HolderId: custody.HolderId,
            CustodyId: custody.Id,
            Description: custody.Purpose,
            CreateUserId: request.CreateUserId,
            BranchId: request.BranchId ?? custody.BranchId,
            ShiftId: request.ShiftId), cancellationToken);

        if (posted.Response is not > 0)
            return new Result(posted.StatusCode, posted.Errors);

        try
        {
            custody.Return(request.Amount, posted.Response);
            await repository.UpdateAsync(custody);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (CustodyDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}
