namespace Inventory.Application.Stocks.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListStockQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<StockDto> , IListQuery<ResultCollection<StockDto>>;

    public sealed class GetListQueryHandler(IRepository<Inventory.Domain.Stock> _Repository, IMapper mapper) : ListCommandHandler<GetListStockQuery, Inventory.Domain.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(GetListStockQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Inventory.Domain.Stock>, IOrderedQueryable<Inventory.Domain.Stock>> CreateOrderBy(GetListStockQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Branch,Account";
        }
    }
}
