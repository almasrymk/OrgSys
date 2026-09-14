namespace Sales.Domain.Events;

public sealed record QuotationCancelledDomainEvent(long QuotationId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
