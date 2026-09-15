namespace Inventory.Domain.Enums;

/// <summary>Shared lifecycle for the simple transactional documents (InventoryReceipt, InventoryIssue,
/// StockAdjustment) — Draft while editable, Posted once it has produced InventoryMovement rows and
/// become immutable (brief §9/§57), Cancelled only while still Draft. StockTransfer uses its own
/// richer StockTransferStatus (brief §11's optional transit workflow).</summary>
public enum DocumentStatus
{
    Draft = 0,
    Confirmed = 10,
    Posted = 20,
    Cancelled = 30
}
