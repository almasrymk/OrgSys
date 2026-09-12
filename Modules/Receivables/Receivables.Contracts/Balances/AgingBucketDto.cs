namespace Receivables.Contracts.Balances;

/// <summary>
/// A customer's outstanding receivable balance, aged into buckets by how long each unpaid amount
/// has been outstanding as of the query's AsOfDate. Computed by FIFO-matching Journal debits
/// (charges) against later Journal credits (receipts) posted to the customer's receivable account
/// — there is no separate per-invoice OpenItem ledger; aging is derived live from the existing
/// Journal/JournalItem history. A negative bucket amount means the customer has a credit balance
/// (overpayment) as of that age.
/// </summary>
public record AgingBucketDto(decimal Current, decimal Days31To60, decimal Days61To90, decimal Over90)
{
    public decimal Total => Current + Days31To60 + Days61To90 + Over90;
}
