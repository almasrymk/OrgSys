namespace Accounting.Domain.Events;

/// <summary>Raised when a Draft Journal is voided (<see cref="Journal.Cancel"/>). Never raised for a Posted journal — see reversal instead.</summary>
public sealed record JournalCancelledDomainEvent(long JournalId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
