namespace Organization.Application.Companies.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchCompanyQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<CompanyDto>, ISearchQuery<ResultPagination<CompanyDto>>;

    public sealed class SearchQueryHandler(IRepository<Organization.Domain.Company> _Repository, IMapper mapper) : SearchCommandHandler<SearchCompanyQuery, Organization.Domain.Company, CompanyDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Company, bool>> CreateFilter(SearchCompanyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.LegalName!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Organization.Domain.Company>, IOrderedQueryable<Organization.Domain.Company>> CreateOrderBy(SearchCompanyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
