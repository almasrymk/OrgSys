namespace Application.Commands.Org.Setting.Role.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListRoleQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<RoleModelView> , IListQuery<ResultCollection<RoleModelView>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Role> _Repository, IMapper mapper) : ListCommandHandler<GetListRoleQuery, Domain.Entities.Role, RoleModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Role, bool>> CreateFilter(GetListRoleQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Role>, IOrderedQueryable<Domain.Entities.Role>> CreateOrderBy(GetListRoleQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}