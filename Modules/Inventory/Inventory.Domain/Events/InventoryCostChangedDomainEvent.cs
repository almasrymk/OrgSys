namespace Inventory.Domain.Events;

/// <summary>Raised by InventoryBalance.Receive when a weighted-average cost recompute changes
/// AverageCost — brief §29. Not raised for FIFO-costed items (their cost is per-layer, not per-balance).</summary>
public sealed record InventoryCostChangedDomainEvent(long ProductId, long StockId, decimal OldAverageCost, decimal NewAverageCost) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
