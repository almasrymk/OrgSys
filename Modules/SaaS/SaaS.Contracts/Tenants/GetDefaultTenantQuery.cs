namespace SaaS.Contracts.Tenants;

using OrgSys.SharedKernel;

/// <summary>Resolves the single seeded "Default Tenant" (docs/architecture/adr/tenant-vs-company.md
/// stage-2 backfill target) — used by Organization/Administration's own retrofit migrations/seeders
/// to assign existing rows a TenantId without hardcoding an id. Mirrors
/// Organization.Contracts.Companies.GetDefaultCompanyQuery / MasterData.Contracts...GetDefaultCurrencyQuery.</summary>
public record GetDefaultTenantQuery() : IQuery<TenantLookupDto?>;
