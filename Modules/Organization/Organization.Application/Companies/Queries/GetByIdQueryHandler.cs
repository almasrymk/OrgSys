namespace Organization.Application.Companies.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using SaaS.Contracts.Tenancy;
    using System.Linq.Expressions;
    using System.Net;

    public sealed record GetByIdCompanyQuery(long Id) : ICommand<CompanyDto>, IGetByIdQuery<Result<CompanyDto>>;

    public sealed class GetByIdQueryHandler(
        IRepository<Organization.Domain.Company> _Repository,
        IMapper mapper,
        ICurrentTenant currentTenant) : GetCommandHandler<GetByIdCompanyQuery, Organization.Domain.Company, CompanyDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Company, bool>> CreateFilter(GetByIdCompanyQuery request)
        {
            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }

        public override async Task<Result<CompanyDto>> Handle(GetByIdCompanyQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (currentTenant.TenantId is long tenantId
                && result.Response is { Id: > 0 }
                && result.Response.TenantId != tenantId)
            {
                return new Result<CompanyDto>(HttpStatusCode.NotFound, null, [new Error("Company not found.")]);
            }

            return result;
        }
    }
}
