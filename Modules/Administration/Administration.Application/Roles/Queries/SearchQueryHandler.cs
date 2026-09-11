namespace Administration.Application.Roles.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchRoleQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<RoleDto> ,ISearchQuery<ResultPagination<RoleDto>>;

    public sealed class SearchQueryHandler(IRepository<Administration.Domain.Role> _Repository, IMapper mapper) : SearchCommandHandler<SearchRoleQuery, Administration.Domain.Role, RoleDto>(_Repository, mapper)
    {
        public override Expression<Func<Administration.Domain.Role, bool>> CreateFilter(SearchRoleQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Administration.Domain.Role>, IOrderedQueryable<Administration.Domain.Role>> CreateOrderBy(SearchRoleQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}