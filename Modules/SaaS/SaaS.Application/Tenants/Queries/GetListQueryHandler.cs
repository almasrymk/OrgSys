namespace SaaS.Application.Tenants.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListTenantQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TenantDto>, IListQuery<ResultCollection<TenantDto>>;

    public sealed class GetListQueryHandler(IRepository<Tenant> _Repository, IMapper mapper) : ListCommandHandler<GetListTenantQuery, Tenant, TenantDto>(_Repository, mapper)
    {
        public override Expression<Func<Tenant, bool>> CreateFilter(GetListTenantQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Tenant>, IOrderedQueryable<Tenant>> CreateOrderBy(GetListTenantQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
