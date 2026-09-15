namespace Inventory.Domain.Events;

public sealed record StockIssuedDomainEvent(long IssueId, long ProductId, long StockId, long? LocationId, decimal Quantity, long? BatchId, long? SerialId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
