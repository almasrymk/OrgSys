namespace Sales.Domain.Events;

public sealed record QuotationCreatedDomainEvent(long QuotationId, long CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
