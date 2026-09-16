namespace Organization.Contracts.Branches;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for batch-resolving Branch display names by id. Handled by
/// Organization.Application. Missing ids are simply absent from the result. Mirrors
/// MasterData.Contracts.Currencies.GetCurrencyNamesQuery.
/// </summary>
public record GetBranchNamesQuery(IReadOnlyCollection<long> BranchIds) : IQuery<Dictionary<long, string?>>;
