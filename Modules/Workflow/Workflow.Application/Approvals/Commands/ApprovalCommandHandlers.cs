namespace Workflow.Application.Approvals.Commands;

using MediatR;
using Purchasing.Contracts.PurchaseRequisitions;
using System.Net;
using Workflow.Contracts.Approvals;
using Workflow.Domain.Exceptions;

public sealed class StartApprovalCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<ApprovalRequest> repository) : ICommandHandler<StartApprovalCommand>
{
    public async Task<Result> Handle(StartApprovalCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByFilterAsync(
            e => e.DocumentType == (WorkflowDocumentType)request.DocumentType
                 && e.DocumentId == request.DocumentId
                 && e.LifecycleStatus == ApprovalStatus.Pending,
            string.Empty);
        if (existing is not null && existing.Id > 0)
            return new Result(HttpStatusCode.OK, null);

        try
        {
            var approval = ApprovalRequest.Start(
                (WorkflowDocumentType)request.DocumentType,
                request.DocumentId,
                request.RequestedByUserId,
                request.RequestedAt);
            await repository.CreateAsync(approval);
        }
        catch (WorkflowDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }

        if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
            return new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);

        return new Result(HttpStatusCode.OK, null);
    }
}

public sealed class DecideApprovalCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<ApprovalRequest> repository,
    ISender sender) : ICommandHandler<DecideApprovalCommand>
{
    public async Task<Result> Handle(DecideApprovalCommand request, CancellationToken cancellationToken)
    {
        var approval = await repository.GetByFilterAsync(e => e.Id == request.ApprovalRequestId, "Decisions");
        if (approval is null || approval.Id == 0)
            return new Result(HttpStatusCode.NotFound, [new Error("Approval request not found.")]);

        try
        {
            approval.Decide(request.ApproverUserId, request.Approved, request.Comment, request.DecidedAt);
        }
        catch (WorkflowDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }

        await repository.UpdateAsync(approval);
        if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
            return new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);

        if (approval.DocumentType == WorkflowDocumentType.PurchaseRequisition)
        {
            var notify = await sender.Send(
                new ApplyPurchaseRequisitionWorkflowDecisionCommand(approval.DocumentId, request.Approved),
                cancellationToken);
            if (notify.StatusCode != HttpStatusCode.OK)
                return notify;
        }

        return new Result(HttpStatusCode.OK, null);
    }
}
