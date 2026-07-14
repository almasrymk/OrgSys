namespace Application.Commands.Org.Setting.Stock.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchStockQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<StockDto> ,ISearchQuery<ResultPagination<StockDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Stock> _Repository, IMapper mapper) : SearchCommandHandler<SearchStockQuery, Domain.Entities.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Stock, bool>> CreateFilter(SearchStockQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Stock>, IOrderedQueryable<Domain.Entities.Stock>> CreateOrderBy(SearchStockQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}