namespace Purchasing.Domain.Events;

public sealed record PurchaseOrderPartiallyReceivedDomainEvent(long PurchaseOrderId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
