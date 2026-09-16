namespace SaaS.Contracts.Tenants;

/// <summary>Public read shape for a Tenant, used by other modules instead of a direct
/// SaaS.Domain.Tenant reference. Mirrors Organization.Contracts.Companies.CompanyLookupDto.</summary>
public record TenantLookupDto(long Id, string Name, string TenantStatus, bool IsActive);
