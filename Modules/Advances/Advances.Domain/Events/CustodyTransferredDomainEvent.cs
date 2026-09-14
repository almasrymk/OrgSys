namespace Advances.Domain.Events;

/// <summary>Raised by Custody.TransferHolder when the accountable holder changes (full transfer only — brief §40).</summary>
public sealed record CustodyTransferredDomainEvent(long CustodyId, long FromHolderId, long ToHolderId, decimal TransferredAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
