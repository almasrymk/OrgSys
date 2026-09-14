namespace Purchasing.Domain.Events;

public sealed record PurchaseRequisitionRejectedDomainEvent(long PurchaseRequisitionId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
