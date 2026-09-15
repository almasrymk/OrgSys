namespace Organization.Application.Companies.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdCompanyQuery(long Id) : ICommand<CompanyDto>, IGetByIdQuery<Result<CompanyDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Organization.Domain.Company> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCompanyQuery, Organization.Domain.Company, CompanyDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Company, bool>> CreateFilter(GetByIdCompanyQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
