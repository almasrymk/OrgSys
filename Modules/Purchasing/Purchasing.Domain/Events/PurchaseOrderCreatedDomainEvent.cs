namespace Purchasing.Domain.Events;

public sealed record PurchaseOrderCreatedDomainEvent(long PurchaseOrderId, long DealerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
