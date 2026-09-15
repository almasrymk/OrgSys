namespace Organization.Application.Companies.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListCompanyQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<CompanyDto>, IListQuery<ResultCollection<CompanyDto>>;

    public sealed class GetListQueryHandler(IRepository<Organization.Domain.Company> _Repository, IMapper mapper) : ListCommandHandler<GetListCompanyQuery, Organization.Domain.Company, CompanyDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Company, bool>> CreateFilter(GetListCompanyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.LegalName!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Organization.Domain.Company>, IOrderedQueryable<Organization.Domain.Company>> CreateOrderBy(GetListCompanyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
