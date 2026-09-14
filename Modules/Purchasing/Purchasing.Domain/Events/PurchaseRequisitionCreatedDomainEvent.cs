namespace Purchasing.Domain.Events;

public sealed record PurchaseRequisitionCreatedDomainEvent(long PurchaseRequisitionId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
