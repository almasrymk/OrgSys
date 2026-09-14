namespace Receivables.Domain.Events;

/// <summary>Raised by Receivable.Apply the moment OutstandingAmount reaches zero (Status -> Settled).</summary>
public sealed record ReceivableSettledDomainEvent(long ReceivableId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
