namespace Sales.Domain.Events;

/// <summary>Raised by SalesOrder.Cancel only on the clean path — cancelling an order with zero
/// delivered quantity so far. See SalesOrderStatus's remark on why a partially-delivered order that
/// has its remainder cancelled raises SalesOrderCompletedDomainEvent instead.</summary>
public sealed record SalesOrderCancelledDomainEvent(long SalesOrderId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
