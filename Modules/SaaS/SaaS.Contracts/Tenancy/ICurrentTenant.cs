namespace SaaS.Contracts.Tenancy;

/// <summary>
/// Request-scoped tenant context resolved from JWT claims. Do not stamp TenantId onto every table;
/// ownership flows through Company.TenantId (and Branch/Stock/User via Company/Branch).
/// </summary>
public interface ICurrentTenant
{
    long? TenantId { get; }

    long? CompanyId { get; }

    long? BranchId { get; }

    bool IsResolved { get; }
}
