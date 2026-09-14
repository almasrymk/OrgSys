namespace Sales.Domain.Events;

public sealed record QuotationExpiredDomainEvent(long QuotationId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
