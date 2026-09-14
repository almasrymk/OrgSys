namespace Payables.Domain;

/// <summary>
/// What kind of business document a Payable was created from. Holds only the sources that
/// currently have a real, wired-up integration (see docs/architecture/payables-ddd-migration.md) —
/// CreditNote/DebitNote/PurchaseReturn are not added until an actual integration for them exists,
/// per the same anti-speculative-enum-member rule the Receivables side established.
/// </summary>
public enum SourceDocumentType
{
    PurchaseInvoice = 1,
    OpeningBalance = 2
}
