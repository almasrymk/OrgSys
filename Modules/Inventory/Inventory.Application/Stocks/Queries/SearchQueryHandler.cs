namespace Inventory.Application.Stocks.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchStockQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<StockDto> ,ISearchQuery<ResultPagination<StockDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.Stock> _Repository, IMapper mapper) : SearchCommandHandler<SearchStockQuery, Inventory.Domain.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(SearchStockQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Inventory.Domain.Stock>, IOrderedQueryable<Inventory.Domain.Stock>> CreateOrderBy(SearchStockQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Branch,Account";
        }
    }
}
