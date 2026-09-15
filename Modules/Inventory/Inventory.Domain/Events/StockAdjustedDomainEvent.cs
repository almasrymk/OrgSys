namespace Inventory.Domain.Events;

public sealed record StockAdjustedDomainEvent(long AdjustmentId, long ProductId, long StockId, MovementDirection Direction, decimal Quantity, long ReasonId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
