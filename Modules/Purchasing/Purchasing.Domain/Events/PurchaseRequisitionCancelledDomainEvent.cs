namespace Purchasing.Domain.Events;

public sealed record PurchaseRequisitionCancelledDomainEvent(long PurchaseRequisitionId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
