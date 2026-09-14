namespace CommercialDocuments.Contracts.IntegrationEvents;

using OrgSys.SharedKernel;

/// <summary>
/// Raised once a Sales Invoice (InvoiceTypeId.Sales) has been posted to the General Ledger via the
/// existing InvoiceJournalPostingService (CommercialDocuments -> Accounting.Contracts) — the sole
/// canonical AR posting path (see docs/architecture/receivables-ddd-migration.md §9). This is a
/// side-effect notification of that existing posting, not a second posting decision: subscribers
/// (currently: Receivables, to build its own open-item projection) must never re-post to the
/// General Ledger from this event.
///
/// DueDate defaults to InvoiceDate — Invoice has no payment-terms/due-date concept today (verified:
/// no such field on CommercialDocuments.Domain.Invoice or Parties.Domain.Dealer); every posted
/// sales invoice is currently treated as due on its own date until payment-terms support exists.
/// </summary>
public sealed record SalesInvoicePostedIntegrationEvent(
    long InvoiceId,
    string? InvoiceNumber,
    long CustomerId,
    DateTime InvoiceDate,
    DateTime DueDate,
    long CurrencyId,
    decimal Rate,
    decimal Amount,
    long CreateUserId,
    DateTime CreateDate,
    long? BranchId) : IntegrationEvent;
