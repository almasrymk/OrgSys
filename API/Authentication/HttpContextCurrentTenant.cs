using System.Security.Claims;
using SaaS.Contracts.Tenancy;

namespace API.Authentication;

public sealed class HttpContextCurrentTenant(IHttpContextAccessor accessor) : ICurrentTenant
{
    public long? TenantId => Parse("tenantId");

    public long? CompanyId => Parse("companyId");

    public long? BranchId => Parse("branchId");

    public bool IsResolved => TenantId.HasValue;

    private long? Parse(string claimType)
    {
        var value = accessor.HttpContext?.User?.FindFirstValue(claimType)
            ?? accessor.HttpContext?.User?.FindFirst(claimType)?.Value;
        return long.TryParse(value, out var id) ? id : null;
    }
}
