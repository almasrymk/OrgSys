namespace Payables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for a supplier's payable aging as of an optional date (defaults to today).
/// Handled by Payables.Application. See <see cref="AgingBucketDto"/> for how buckets are computed.
/// </summary>
public record GetSupplierAgingQuery(long DealerId, DateTime? AsOfDate) : IQuery<AgingBucketDto>;
