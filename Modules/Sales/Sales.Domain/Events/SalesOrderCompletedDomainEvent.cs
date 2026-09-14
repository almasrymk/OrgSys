namespace Sales.Domain.Events;

/// <summary>Raised whenever a SalesOrder has nothing left to deliver — reached either through full
/// delivery, or by cancelling the remaining quantity of an order that already had some delivery
/// history (see SalesOrderStatus's remark on why there is no separate "Completed" status for this).</summary>
public sealed record SalesOrderCompletedDomainEvent(long SalesOrderId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
