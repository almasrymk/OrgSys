namespace Workflow.Contracts.Approvals;

using OrgSys.SharedKernel;

public sealed record StartApprovalCommand(
    WorkflowDocumentTypeCode DocumentType,
    long DocumentId,
    long RequestedByUserId,
    DateTime RequestedAt) : ICommand;

public sealed record DecideApprovalCommand(
    long ApprovalRequestId,
    long ApproverUserId,
    bool Approved,
    string? Comment,
    DateTime DecidedAt) : ICommand;

public sealed record ApprovalRequestDto(
    long Id,
    WorkflowDocumentTypeCode DocumentType,
    long DocumentId,
    int LifecycleStatus,
    long RequestedByUserId,
    DateTime RequestedAt);

public sealed record GetApprovalByDocumentQuery(
    WorkflowDocumentTypeCode DocumentType,
    long DocumentId) : IQuery<ApprovalRequestDto?>;

public enum WorkflowDocumentTypeCode
{
    PurchaseRequisition = 1
}
