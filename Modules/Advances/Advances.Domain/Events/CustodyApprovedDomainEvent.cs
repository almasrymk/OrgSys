namespace Advances.Domain.Events;

/// <summary>Raised by Custody.Approve when a Draft custody request is approved.</summary>
public sealed record CustodyApprovedDomainEvent(long CustodyId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
