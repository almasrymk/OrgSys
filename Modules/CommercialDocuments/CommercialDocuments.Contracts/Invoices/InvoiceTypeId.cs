namespace CommercialDocuments.Contracts.Invoices;

/// <summary>
/// Semantic names for the existing Invoice.TypeId discriminator values. These are the fixed,
/// already-in-production database IDs (InvoiceType table) — do not renumber them. Callers outside
/// CommercialDocuments should use these names instead of bare integer literals (e.g. `TypeId == 2`).
/// </summary>
public enum InvoiceTypeId
{
    Sales = 1,
    Purchase = 2,
    SalesReturn = 3,
    PurchaseReturn = 4,
}
