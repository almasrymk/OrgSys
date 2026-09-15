namespace Organization.Contracts.Companies;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for batch-resolving Company display names by id. Handled by
/// Organization.Application. Missing ids are simply absent from the result. Mirrors
/// MasterData.Contracts.Currencies.GetCurrencyNamesQuery.
/// </summary>
public record GetCompanyNamesQuery(IReadOnlyCollection<long> CompanyIds) : IQuery<Dictionary<long, string?>>;
