namespace Inventory.Domain;

/// <summary>
/// What kind of document a posted InventoryMovement (Transaction) or a StockReservation was created
/// from — replaces the previous string-based correlation (`Notes = "InventoryId: {id}"`) with a
/// structured, queryable reference (brief §31/§55). Local to Inventory.Domain, mirroring
/// Payables.Domain.Enums.SourceDocumentType / Receivables.Domain.Enums.SourceDocumentType — holds
/// only sources with a real, wired-up integration; no speculative members.
/// </summary>
public enum SourceDocumentType
{
    /// <summary>A Sales or Purchase Invoice (CommercialDocuments.Domain.Invoice) via CreateTransactionByInvoiceCommand.</summary>
    Invoice = 1,

    /// <summary>A physical stock count (Inventory.Domain.Inventory) posting its counted variance.</summary>
    StockCount = 2,

    InventoryReceipt = 3,

    InventoryIssue = 4,

    StockTransfer = 5,

    StockAdjustment = 6,

    /// <summary>A compensating movement created by Transaction.Reverse() — see brief §56.</summary>
    Reversal = 7,

    /// <summary>A sales-order reservation request (Sales.Contracts caller) — reservation-only, never posts a movement by itself.</summary>
    SalesOrder = 8
}
