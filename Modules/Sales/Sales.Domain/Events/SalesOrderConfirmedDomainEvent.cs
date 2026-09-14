namespace Sales.Domain.Events;

/// <summary>Raised by SalesOrder.Confirm — the intended trigger for Application to request Inventory
/// stock reservation (brief §39), never done by the aggregate itself.</summary>
public sealed record SalesOrderConfirmedDomainEvent(long SalesOrderId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
