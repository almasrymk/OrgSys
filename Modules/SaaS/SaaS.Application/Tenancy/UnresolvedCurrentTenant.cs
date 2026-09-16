namespace SaaS.Application.Tenancy;

using SaaS.Contracts.Tenancy;

public sealed class UnresolvedCurrentTenant : ICurrentTenant
{
    public long? TenantId => null;

    public long? CompanyId => null;

    public long? BranchId => null;

    public bool IsResolved => false;
}
