namespace Accounting.Domain.Events;

/// <summary>Raised on the original Journal when it is reversed by a new, linked Journal (<see cref="Journal.CreateReversal"/>).</summary>
public sealed record JournalReversedDomainEvent(long OriginalJournalId, long ReversalJournalId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
