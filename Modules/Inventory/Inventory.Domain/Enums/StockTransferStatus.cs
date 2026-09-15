namespace Inventory.Domain.Enums;

/// <summary>
/// StockTransfer's own lifecycle (brief §11) — richer than DocumentStatus because a transfer can
/// pass through an in-transit state between the source issue and destination receipt, matching the
/// existing TransferIssue(3)/TransferReceipt(4) pairing's two-sided posting.
/// </summary>
public enum StockTransferStatus
{
    Draft = 0,
    Confirmed = 10,
    Shipped = 20,
    Received = 30,
    Completed = 40,
    Cancelled = 50
}
