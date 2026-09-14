namespace Receivables.Domain.Events;

/// <summary>Raised by Receivable.Cancel. Only reachable from Open (no payment applied yet) — see ReceivableCannotBeCancelledException.</summary>
public sealed record ReceivableCancelledDomainEvent(long ReceivableId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
