namespace Advances.Domain.Events;

/// <summary>Raised by Custody.Cancel when a Draft (never-issued) custody request is cancelled.</summary>
public sealed record CustodyCancelledDomainEvent(long CustodyId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
