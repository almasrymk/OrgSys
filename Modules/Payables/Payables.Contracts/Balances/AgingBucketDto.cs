namespace Payables.Contracts.Balances;

/// <summary>
/// A supplier's outstanding payable balance, aged into buckets by how long each unpaid amount has
/// been outstanding as of the query's AsOfDate. Mirrors Receivables.Contracts.Balances.AgingBucketDto
/// for the AP side — see that type for the FIFO-matching computation this is derived from.
/// </summary>
public record AgingBucketDto(decimal Current, decimal Days31To60, decimal Days61To90, decimal Over90)
{
    public decimal Total => Current + Days31To60 + Days61To90 + Over90;
}
