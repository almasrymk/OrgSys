namespace CommercialDocuments.Contracts.Invoices;

/// <summary>
/// Minimal public shape for another module to verify an Invoice exists and to branch on its type
/// — e.g. Purchasing confirming a linked document is a Purchase/PurchaseReturn invoice before
/// attaching it to a PurchaseOrder. Consumers must not reach into CommercialDocuments.Domain.Invoice
/// directly; this is the contract surface instead.
/// </summary>
public sealed record InvoiceReferenceDto(
    long Id,
    string? Code,
    InvoiceTypeId TypeId,
    long DealerId,
    bool IsDeleted);
