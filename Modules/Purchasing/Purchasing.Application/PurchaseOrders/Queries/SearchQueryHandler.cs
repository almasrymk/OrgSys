namespace Purchasing.Application.PurchaseOrders.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchPurchaseOrderQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandPagination<PurchaseOrderDto>, ISearchQuery<ResultPagination<PurchaseOrderDto>>;

    public sealed class SearchQueryHandler(IRepository<PurchaseOrder> _Repository, IMapper mapper)
        : SearchCommandHandler<SearchPurchaseOrderQuery, PurchaseOrder, PurchaseOrderDto>(_Repository, mapper)
    {
        public override Expression<Func<PurchaseOrder, bool>> CreateFilter(SearchPurchaseOrderQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch) || e.Dealer!.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<PurchaseOrder>, IOrderedQueryable<PurchaseOrder>> CreateOrderBy(SearchPurchaseOrderQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Dealer";
        }
    }
}
