namespace Sales.Domain.Events;

public sealed record QuotationRejectedDomainEvent(long QuotationId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
