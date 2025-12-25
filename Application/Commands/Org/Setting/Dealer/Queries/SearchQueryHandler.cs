namespace Application.Commands.Org.Setting.Dealer.Queries
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

    public sealed record SearchDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DealerModelView> ,ISearchQuery<ResultPagination<DealerModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Dealer> _Repository, IMapper mapper) : SearchCommandHandler<SearchDealerQuery, Entity.Model.Dealer, DealerModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Dealer, bool>> CreateFilter(SearchDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Dealer>, IOrderedQueryable<Entity.Model.Dealer>> CreateOrderBy(SearchDealerQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}