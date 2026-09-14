namespace Sales.Domain.Events;

public sealed record QuotationSentDomainEvent(long QuotationId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
