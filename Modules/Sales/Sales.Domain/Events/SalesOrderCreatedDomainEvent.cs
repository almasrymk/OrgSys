namespace Sales.Domain.Events;

public sealed record SalesOrderCreatedDomainEvent(long SalesOrderId, long CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
