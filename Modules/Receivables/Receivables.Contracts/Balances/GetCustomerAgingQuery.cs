namespace Receivables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for a customer's receivable aging as of an optional date (defaults to today).
/// Handled by Receivables.Application. See <see cref="AgingBucketDto"/> for how buckets are
/// computed.
/// </summary>
public record GetCustomerAgingQuery(long DealerId, DateTime? AsOfDate) : IQuery<AgingBucketDto>;
