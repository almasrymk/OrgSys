namespace Application.Commands.Org.Setting.Account.Queries
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

    public sealed record SearchAccountQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<AccountModelView> ,ISearchQuery<ResultPagination<AccountModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Account> _Repository, IMapper mapper) : SearchCommandHandler<SearchAccountQuery, Entity.Model.Account, AccountModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Account, bool>> CreateFilter(SearchAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Account>, IOrderedQueryable<Entity.Model.Account>> CreateOrderBy(SearchAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}