namespace Payables.Domain.Events;

/// <summary>Raised by Payable.Cancel. Only reachable from Open (no payment applied yet) — see PayableCannotBeCancelledException.</summary>
public sealed record PayableCancelledDomainEvent(long PayableId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
