namespace CommercialDocuments.Contracts.IntegrationEvents;

using OrgSys.SharedKernel;

/// <summary>
/// Raised once a Purchase Invoice (InvoiceTypeId.Purchase) has been posted to the General Ledger
/// via the existing InvoiceJournalPostingService (CommercialDocuments -> Accounting.Contracts) —
/// the sole canonical AP posting path, the same bridge SalesInvoicePostedIntegrationEvent uses on
/// the AR side (credit side instead of debit — see InvoiceJournalPostingService's dealerIsDebit
/// branch). This is a side-effect notification of that existing posting, not a second posting
/// decision: subscribers (currently: Payables, to create its own open-item projection) must never
/// re-post to the GL from this event.
///
/// DueDate defaults to InvoiceDate — Invoice has no payment-terms/due-date concept today, the same
/// gap SalesInvoicePostedIntegrationEvent documents on the AR side.
/// </summary>
public sealed record PurchaseInvoicePostedIntegrationEvent(
    long InvoiceId,
    string? InvoiceNumber,
    long SupplierId,
    DateTime InvoiceDate,
    DateTime DueDate,
    long CurrencyId,
    decimal Rate,
    decimal Amount,
    long CreateUserId,
    DateTime CreateDate,
    long? BranchId) : IntegrationEvent;
