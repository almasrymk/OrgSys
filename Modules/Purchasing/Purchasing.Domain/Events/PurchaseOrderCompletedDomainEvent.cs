namespace Purchasing.Domain.Events;

/// <summary>Raised whenever a PurchaseOrder has nothing left to receive — reached either through
/// full receipt, or by cancelling the remaining quantity of an order that already had some receipt
/// history (mirrors Sales.Domain.Events.SalesOrderCompletedDomainEvent's identical reasoning).</summary>
public sealed record PurchaseOrderCompletedDomainEvent(long PurchaseOrderId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
