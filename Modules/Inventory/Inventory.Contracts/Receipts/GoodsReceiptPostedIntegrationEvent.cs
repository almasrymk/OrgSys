namespace Inventory.Contracts.Receipts;

using OrgSys.SharedKernel;

public sealed record GoodsReceiptPostedLine(long ProductId, long UnitId, decimal Quantity);

public sealed record GoodsReceiptPostedIntegrationEvent(
    long InventoryReceiptId,
    long StockId,
    long? SourceId,
    DateTime PostedAt,
    long? PurchaseOrderId = null,
    IReadOnlyList<GoodsReceiptPostedLine>? Lines = null) : IntegrationEvent;
