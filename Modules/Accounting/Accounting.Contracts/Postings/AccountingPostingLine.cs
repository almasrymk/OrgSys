namespace Accounting.Contracts.Postings;

/// <summary>
/// One debit/credit line of a document's accounting posting. The source module (CommercialDocuments,
/// Inventory, ...) resolves which accounts apply under its own business rules (dealer/tax/stock
/// accounts, etc.); Accounting only assembles and persists the Journal from the resolved lines —
/// see PostAccountingDocumentCommand.
/// </summary>
public sealed record AccountingPostingLine(long AccountId, decimal Debit, decimal Credit, string? Note = null);
