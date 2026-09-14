namespace Sales.Domain;

/// <summary>
/// Lifecycle of one Quotation. Deliberately leaner than a generic approval workflow (Draft -> Sent
/// -> Accepted/Rejected/Expired -> Converted; Draft/Sent -> Cancelled) — nothing in the current
/// OrgSys codebase has an internal Submitted/Approved pre-send step for any commercial document
/// (Financial/Invoice both post directly), so this is not carried over speculatively. Mirrors the
/// same "don't add workflow states the business doesn't have yet" rule Advances.Domain.CustodyStatus
/// documents for its own lifecycle.
/// </summary>
public enum QuotationStatus
{
    Draft = 0,
    Sent = 10,
    Accepted = 20,
    Rejected = 30,
    Expired = 40,
    Converted = 50,
    Cancelled = 60
}
