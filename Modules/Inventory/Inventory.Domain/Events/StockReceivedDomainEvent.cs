namespace Inventory.Domain.Events;

public sealed record StockReceivedDomainEvent(long ReceiptId, long ProductId, long StockId, long? LocationId, decimal Quantity, decimal UnitCost, long? BatchId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
