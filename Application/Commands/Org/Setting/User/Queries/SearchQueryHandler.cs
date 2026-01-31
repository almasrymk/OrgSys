namespace Application.Commands.Org.Setting.User.Queries
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

    public sealed record SearchUserQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<UserModelView> ,ISearchQuery<ResultPagination<UserModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.User> _Repository, IMapper mapper) : SearchCommandHandler<SearchUserQuery, Entity.Model.User, UserModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.User, bool>> CreateFilter(SearchUserQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.User>, IOrderedQueryable<Entity.Model.User>> CreateOrderBy(SearchUserQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Role,Branch";
        }
    }
}