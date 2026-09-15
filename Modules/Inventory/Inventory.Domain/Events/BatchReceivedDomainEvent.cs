namespace Inventory.Domain.Events;

public sealed record BatchReceivedDomainEvent(long BatchId, long ProductId, string BatchNumber, DateTime? ExpiryDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
