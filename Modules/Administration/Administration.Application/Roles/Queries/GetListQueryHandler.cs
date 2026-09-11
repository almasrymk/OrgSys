namespace Administration.Application.Roles.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListRoleQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<RoleDto> , IListQuery<ResultCollection<RoleDto>>;

    public sealed class GetListQueryHandler(IRepository<Administration.Domain.Role> _Repository, IMapper mapper) : ListCommandHandler<GetListRoleQuery, Administration.Domain.Role, RoleDto>(_Repository, mapper)
    {
        public override Expression<Func<Administration.Domain.Role, bool>> CreateFilter(GetListRoleQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Administration.Domain.Role>, IOrderedQueryable<Administration.Domain.Role>> CreateOrderBy(GetListRoleQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}