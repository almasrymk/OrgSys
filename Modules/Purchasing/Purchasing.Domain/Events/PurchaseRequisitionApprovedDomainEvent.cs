namespace Purchasing.Domain.Events;

/// <summary>Raised by PurchaseRequisition.RecordSourced once the requisition has been (fully or
/// partially) converted into a PurchaseOrder.</summary>
public sealed record PurchaseRequisitionApprovedDomainEvent(long PurchaseRequisitionId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
