namespace Organization.Contracts.Companies;

using OrgSys.SharedKernel;

public sealed record CompanyOwnershipDto(long CompanyId, long? TenantId);

public sealed record GetCompanyOwnershipQuery(long CompanyId) : IQuery<CompanyOwnershipDto?>;

public sealed record BranchOwnershipDto(long BranchId, long CompanyId, long? TenantId);

public sealed record GetBranchOwnershipQuery(long BranchId) : IQuery<BranchOwnershipDto?>;

public sealed record TenantOrganizationScopeDto(
    IReadOnlyList<long> CompanyIds,
    IReadOnlyList<long> BranchIds);

public sealed record GetTenantOrganizationScopeQuery(long TenantId) : IQuery<TenantOrganizationScopeDto>;
