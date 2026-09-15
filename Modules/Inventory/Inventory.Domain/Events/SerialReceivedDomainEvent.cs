namespace Inventory.Domain.Events;

public sealed record SerialReceivedDomainEvent(long SerialId, long ProductId, string SerialNumber, long StockId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
