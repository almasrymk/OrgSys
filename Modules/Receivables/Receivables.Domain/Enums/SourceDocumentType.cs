namespace Receivables.Domain;

/// <summary>
/// What kind of business document a Receivable was created from. Holds only the sources that
/// currently have a real, wired-up integration (see docs/architecture/receivables-ddd-migration.md
/// §9/§11) — ManualAdjustment/CreditNote/DebitNote are not added until an actual integration for
/// them exists, per the migration doc's anti-speculative-enum-member rule.
/// </summary>
public enum SourceDocumentType
{
    SalesInvoice = 1,
    OpeningBalance = 2
}
