namespace SaaS.Contracts.Tenants;

using OrgSys.SharedKernel;

/// <summary>Batch-resolves Tenant display names by id. Mirrors
/// Organization.Contracts.Companies.GetCompanyNamesQuery.</summary>
public record GetTenantNamesQuery(IReadOnlyCollection<long> TenantIds) : IQuery<Dictionary<long, string?>>;
