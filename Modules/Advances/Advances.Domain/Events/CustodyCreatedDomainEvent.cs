namespace Advances.Domain.Events;

/// <summary>Raised by Custody.Create when a new Draft custody request is created.</summary>
public sealed record CustodyCreatedDomainEvent(long CustodyId, long HolderId, decimal IssuedAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
