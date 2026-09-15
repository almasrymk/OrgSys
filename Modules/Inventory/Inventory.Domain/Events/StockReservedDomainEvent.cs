namespace Inventory.Domain.Events;

public sealed record StockReservedDomainEvent(long ProductId, long StockId, long? LocationId, decimal Quantity, SourceDocumentType SourceType, long SourceId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
