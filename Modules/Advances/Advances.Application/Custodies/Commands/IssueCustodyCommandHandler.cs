namespace Advances.Application.Custodies.Commands;

using Advances.Domain.Exceptions;
using MediatR;
using System.Net;
using Treasury.Contracts.Financials;

public sealed record IssueCustodyCommand(
    long Id,
    long FinancialAccountId,
    long CounterAccountId,
    DateTime IssueDate,
    long CreateUserId,
    long? BranchId,
    long? ShiftId) : ICommand;

public sealed class IssueCustodyCommandHandler(
    IRepository<Custody> repository,
    IUnitOfWork unitOfWork,
    ISender sender)
    : ICommandHandler<IssueCustodyCommand>
{
    public async Task<Result> Handle(IssueCustodyCommand request, CancellationToken cancellationToken)
    {
        var custody = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, string.Empty);
        if (custody is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Custody not found.")]);

        if (custody.LifecycleStatus != CustodyStatus.Approved)
            return new Result(HttpStatusCode.BadRequest, [new Error("Only an Approved custody can be issued.")]);

        var posted = await sender.Send(new PostCustodyFinancialCommand(
            Disbursement: true,
            FinancialAccountId: request.FinancialAccountId,
            CounterAccountId: request.CounterAccountId,
            Amount: custody.IssuedAmount,
            CurrencyId: custody.CurrencyId,
            ExchangeRate: custody.Rate,
            TransactionDate: request.IssueDate == default ? DateTime.Now : request.IssueDate,
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
            custody.MarkIssued(posted.Response, request.IssueDate == default ? DateTime.Now : request.IssueDate);
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
