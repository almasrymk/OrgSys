namespace Inventory.Application.InventoryBalances.Queries;

public sealed record StockBalanceDto(
    long ProductId, long StockId, long? LocationId, long? BatchId,
    decimal QuantityOnHand, decimal QuantityReserved, decimal QuantityAvailable, decimal AverageCost, decimal InventoryValue);
