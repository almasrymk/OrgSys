namespace Organization.Application.Companies.Queries;

using OrgSys.SharedKernel;
using Organization.Contracts.Companies;
using System.Net;

public sealed class GetCompanyOwnershipQueryHandler(IRepository<Organization.Domain.Company> repository)
    : IQueryHandler<GetCompanyOwnershipQuery, CompanyOwnershipDto?>
{
    public async Task<Result<CompanyOwnershipDto?>> Handle(GetCompanyOwnershipQuery request, CancellationToken cancellationToken)
    {
        var company = await repository.GetByFilterAsync(e => e.Id == request.CompanyId, string.Empty);
        if (company is null || company.Id == 0)
            return new Result<CompanyOwnershipDto?>(HttpStatusCode.OK, null, null);

        return new Result<CompanyOwnershipDto?>(
            HttpStatusCode.OK,
            new CompanyOwnershipDto(company.Id, company.TenantId),
            null);
    }
}

public sealed class GetBranchOwnershipQueryHandler(
    IRepository<Organization.Domain.Branch> branchRepository,
    IRepository<Organization.Domain.Company> companyRepository)
    : IQueryHandler<GetBranchOwnershipQuery, BranchOwnershipDto?>
{
    public async Task<Result<BranchOwnershipDto?>> Handle(GetBranchOwnershipQuery request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByFilterAsync(e => e.Id == request.BranchId, string.Empty);
        if (branch is null || branch.Id == 0)
            return new Result<BranchOwnershipDto?>(HttpStatusCode.OK, null, null);

        var company = await companyRepository.GetByFilterAsync(e => e.Id == branch.CompanyId, string.Empty);
        return new Result<BranchOwnershipDto?>(
            HttpStatusCode.OK,
            new BranchOwnershipDto(branch.Id, branch.CompanyId, company?.TenantId),
            null);
    }
}

public sealed class GetTenantOrganizationScopeQueryHandler(
    IRepository<Organization.Domain.Company> companyRepository,
    IRepository<Organization.Domain.Branch> branchRepository)
    : IQueryHandler<GetTenantOrganizationScopeQuery, TenantOrganizationScopeDto>
{
    public async Task<Result<TenantOrganizationScopeDto>> Handle(GetTenantOrganizationScopeQuery request, CancellationToken cancellationToken)
    {
        var companies = (await companyRepository.GetListByFilterAsync(c => c.TenantId == request.TenantId) ?? [])
            .Where(c => c.Status != Status.Deleted)
            .ToList();
        var companyIds = companies.Select(c => c.Id).ToList();
        var branches = companyIds.Count == 0
            ? []
            : (await branchRepository.GetListByFilterAsync(b => companyIds.Contains(b.CompanyId)) ?? []).ToList();

        return new Result<TenantOrganizationScopeDto>(
            HttpStatusCode.OK,
            new TenantOrganizationScopeDto(companyIds, branches.Select(b => b.Id).ToList()),
            null);
    }
}
