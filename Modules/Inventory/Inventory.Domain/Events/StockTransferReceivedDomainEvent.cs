namespace Inventory.Domain.Events;

public sealed record StockTransferReceivedDomainEvent(long TransferId, long FromStockId, long ToStockId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
