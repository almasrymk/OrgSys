namespace Purchasing.Domain.Events;

public sealed record PurchaseRequisitionSubmittedDomainEvent(long PurchaseRequisitionId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
