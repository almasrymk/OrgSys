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

    public sealed record SearchRoleQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<RoleModelView> ,ISearchQuery<ResultPagination<RoleModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Role> _Repository, IMapper mapper) : SearchCommandHandler<SearchRoleQuery, Domain.Entities.Role, RoleModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Role, bool>> CreateFilter(SearchRoleQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Role>, IOrderedQueryable<Domain.Entities.Role>> CreateOrderBy(SearchRoleQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}