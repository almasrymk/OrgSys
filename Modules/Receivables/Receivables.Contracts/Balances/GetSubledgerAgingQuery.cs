namespace Receivables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// A customer's receivable aging computed directly from the AR subledger's own Receivable rows
/// (bucketed by each open item's own DueDate as of AsOfDate) — as opposed to GetCustomerAgingQuery,
/// which FIFO-matches GL Journal debits/credits live. Only reflects Receivables created since this
/// subledger started recording them (Phase 4/5 onward) — see
/// docs/architecture/receivables-ddd-migration.md §12 for why historical backfill is out of scope.
/// </summary>
public record GetSubledgerAgingQuery(long CustomerId, DateTime? AsOfDate) : IQuery<AgingBucketDto>;
