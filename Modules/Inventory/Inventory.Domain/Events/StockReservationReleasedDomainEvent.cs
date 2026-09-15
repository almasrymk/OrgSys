namespace Inventory.Domain.Events;

public sealed record StockReservationReleasedDomainEvent(long ProductId, long StockId, decimal Quantity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
