namespace OrgSys.Messaging;

using OrgSys.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("OutboxMessage")]
public class OutboxMessage : BaseModel
{
    public virtual Guid EventId { get; set; }

    [StringLength(500)]
    public virtual string EventType { get; set; } = string.Empty;

    public virtual string Payload { get; set; } = string.Empty;

    public virtual DateTime OccurredOn { get; set; }

    public virtual DateTime? ProcessedOn { get; set; }

    [StringLength(1000)]
    public virtual string? Error { get; set; }
}

[Table("InboxMessage")]
public class InboxMessage : BaseModel
{
    public virtual Guid EventId { get; set; }

    [StringLength(200)]
    public virtual string HandlerName { get; set; } = string.Empty;

    public virtual DateTime ProcessedOn { get; set; }
}
