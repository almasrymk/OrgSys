namespace SaaS.Application.Tenants.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchTenantQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<TenantDto>, ISearchQuery<ResultPagination<TenantDto>>;

    public sealed class SearchQueryHandler(IRepository<Tenant> _Repository, IMapper mapper) : SearchCommandHandler<SearchTenantQuery, Tenant, TenantDto>(_Repository, mapper)
    {
        public override Expression<Func<Tenant, bool>> CreateFilter(SearchTenantQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Tenant>, IOrderedQueryable<Tenant>> CreateOrderBy(SearchTenantQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
