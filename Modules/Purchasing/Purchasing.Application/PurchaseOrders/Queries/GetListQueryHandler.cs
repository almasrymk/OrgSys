namespace Purchasing.Application.PurchaseOrders.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListPurchaseOrderQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<PurchaseOrderDto>, IListQuery<ResultCollection<PurchaseOrderDto>>;

    public sealed class GetListQueryHandler(IRepository<PurchaseOrder> _Repository, IMapper mapper)
        : ListCommandHandler<GetListPurchaseOrderQuery, PurchaseOrder, PurchaseOrderDto>(_Repository, mapper)
    {
        public override Expression<Func<PurchaseOrder, bool>> CreateFilter(GetListPurchaseOrderQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch) || e.Dealer!.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<PurchaseOrder>, IOrderedQueryable<PurchaseOrder>> CreateOrderBy(GetListPurchaseOrderQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Dealer";
        }
    }
}
