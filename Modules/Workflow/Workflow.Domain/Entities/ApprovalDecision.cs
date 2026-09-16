namespace Workflow.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ApprovalDecision")]
public class ApprovalDecision : BaseModel
{
    public virtual long ApprovalRequestId { get; private set; }

    public virtual long ApproverUserId { get; private set; }

    public virtual bool IsApproved { get; private set; }

    [StringLength(500)]
    public virtual string? Comment { get; private set; }

    public virtual DateTime DecidedAt { get; private set; }

    public virtual ApprovalRequest? ApprovalRequest { get; private set; }

    protected ApprovalDecision() { }

    internal ApprovalDecision(long approvalRequestId, long approverUserId, bool isApproved, string? comment, DateTime decidedAt)
    {
        ApprovalRequestId = approvalRequestId;
        ApproverUserId = approverUserId;
        IsApproved = isApproved;
        Comment = comment;
        DecidedAt = decidedAt;
    }
}
