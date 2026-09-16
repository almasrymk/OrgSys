namespace Workflow.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ApprovalRequest")]
public class ApprovalRequest : BaseModel
{
    private readonly List<ApprovalDecision> _decisions = [];
    private readonly List<IDomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public virtual WorkflowDocumentType DocumentType { get; private set; }

    public virtual long DocumentId { get; private set; }

    public virtual ApprovalStatus LifecycleStatus { get; private set; }

    public virtual long RequestedByUserId { get; private set; }

    public virtual DateTime RequestedAt { get; private set; }

    public virtual IReadOnlyCollection<ApprovalDecision> Decisions => _decisions.AsReadOnly();

    protected ApprovalRequest() { }

    public static ApprovalRequest Start(WorkflowDocumentType documentType, long documentId, long requestedByUserId, DateTime requestedAt)
    {
        if (documentId <= 0)
            throw new Exceptions.WorkflowDomainException("A workflow document id is required.");

        var request = new ApprovalRequest
        {
            DocumentType = documentType,
            DocumentId = documentId,
            RequestedByUserId = requestedByUserId,
            RequestedAt = requestedAt,
            LifecycleStatus = ApprovalStatus.Pending
        };
        return request;
    }

    public ApprovalDecision Decide(long approverUserId, bool approved, string? comment, DateTime decidedAt)
    {
        if (LifecycleStatus != ApprovalStatus.Pending)
            throw new Exceptions.WorkflowDomainException("Only a pending approval request can be decided.");

        var decision = new ApprovalDecision(Id, approverUserId, approved, comment, decidedAt);
        _decisions.Add(decision);
        LifecycleStatus = approved ? ApprovalStatus.Approved : ApprovalStatus.Rejected;
        return decision;
    }
}
