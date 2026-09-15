namespace Organization.Contracts.Companies;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for resolving "the" Company other modules should default to when no explicit
/// CompanyId is supplied — useful while OrgSys has no real multi-company UI yet (single seeded
/// Company). Returns the first non-hidden, non-deleted Company (by Id), or null if none exist.
/// Handled by Organization.Application. Mirrors MasterData.Contracts.Currencies.GetDefaultCurrencyQuery.
/// </summary>
public record GetDefaultCompanyQuery : IQuery<CompanyLookupDto?>;
