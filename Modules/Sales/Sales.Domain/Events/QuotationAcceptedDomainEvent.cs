namespace Sales.Domain.Events;

public sealed record QuotationAcceptedDomainEvent(long QuotationId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
