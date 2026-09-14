namespace Payables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// A supplier's payable aging computed directly from the AP subledger's own Payable rows (bucketed
/// by each open item's own DueDate as of AsOfDate) — as opposed to GetSupplierAgingQuery, which
/// FIFO-matches GL Journal debits/credits live. Only reflects Payables created since this subledger
/// started recording them.
/// </summary>
public record GetSubledgerAgingQuery(long SupplierId, DateTime? AsOfDate) : IQuery<AgingBucketDto>;
