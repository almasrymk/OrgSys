namespace Inventory.Application.InventoryReceipts.Queries;

public sealed record InventoryReceiptLineDto(long Id, long RowNumber, long ProductId, long UnitId, decimal Quantity, decimal UnitCost, long? BatchId, string? Notes);

public sealed record InventoryReceiptDto(
    long Id, string? Code, long StockId, long? LocationId, long? DealerId, DateTime Date,
    DocumentStatus LifecycleStatus, string? Notes, List<InventoryReceiptLineDto> Lines);
