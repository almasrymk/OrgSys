namespace Workflow.Application.Approvals.Queries;

using System.Net;
using Workflow.Contracts.Approvals;

public sealed class GetApprovalByDocumentQueryHandler(IRepository<ApprovalRequest> repository)
    : IQueryHandler<GetApprovalByDocumentQuery, ApprovalRequestDto?>
{
    public async Task<Result<ApprovalRequestDto?>> Handle(GetApprovalByDocumentQuery request, CancellationToken cancellationToken)
    {
        var approval = await repository.GetByFilterAsync(
            e => e.DocumentType == (WorkflowDocumentType)request.DocumentType && e.DocumentId == request.DocumentId,
            string.Empty);

        if (approval is null || approval.Id == 0)
            return new Result<ApprovalRequestDto?>(HttpStatusCode.OK, null, null);

        return new Result<ApprovalRequestDto?>(
            HttpStatusCode.OK,
            new ApprovalRequestDto(
                approval.Id,
                (WorkflowDocumentTypeCode)approval.DocumentType,
                approval.DocumentId,
                (int)approval.LifecycleStatus,
                approval.RequestedByUserId,
                approval.RequestedAt),
            null);
    }
}
