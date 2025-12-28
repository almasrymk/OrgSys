namespace Application.Commands.Org.Setting.Role.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListRoleQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<RoleModelView> , IListQuery<ResultCollection<RoleModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Role> _Repository, IMapper mapper) : ListCommandHandler<GetListRoleQuery, Entity.Model.Role, RoleModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Role, bool>> CreateFilter(GetListRoleQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Role>, IOrderedQueryable<Entity.Model.Role>> CreateOrderBy(GetListRoleQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}