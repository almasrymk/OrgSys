namespace Parties.Contracts.Dealers;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for batch-resolving Dealer display names by id (e.g. report/list screens
/// showing DealerName without an EF Include across the module boundary). Handled by
/// Parties.Application. Missing ids are simply absent from the result.
/// </summary>
public record GetDealerNamesQuery(IReadOnlyCollection<long> DealerIds) : IQuery<Dictionary<long, string?>>;
