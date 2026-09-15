namespace Inventory.Domain.Events;

public sealed record StockTransferShippedDomainEvent(long TransferId, long FromStockId, long ToStockId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
