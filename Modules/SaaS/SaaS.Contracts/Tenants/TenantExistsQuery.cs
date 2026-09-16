namespace SaaS.Contracts.Tenants;

using OrgSys.SharedKernel;

public record TenantExistsQuery(long TenantId) : IQuery<bool>;
