namespace Purchasing.Domain.Events;

public sealed record PurchaseOrderCancelledDomainEvent(long PurchaseOrderId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
