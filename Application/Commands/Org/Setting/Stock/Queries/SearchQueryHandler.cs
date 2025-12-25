namespace Application.Commands.Org.Setting.Stock.Queries
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

    public sealed record SearchStockQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<StockModelView> ,ISearchQuery<ResultPagination<StockModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Stock> _Repository, IMapper mapper) : SearchCommandHandler<SearchStockQuery, Entity.Model.Stock, StockModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Stock, bool>> CreateFilter(SearchStockQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Stock>, IOrderedQueryable<Entity.Model.Stock>> CreateOrderBy(SearchStockQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}