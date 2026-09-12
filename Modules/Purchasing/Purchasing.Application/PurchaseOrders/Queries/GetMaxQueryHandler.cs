namespace Purchasing.Application.PurchaseOrders.Queries
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record GetMaxPurchaseOrderQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<PurchaseOrder> _Repository)
        : GetMaxCommandHandler<GetMaxPurchaseOrderQuery, PurchaseOrder>(_Repository)
    {
        public override Expression<Func<PurchaseOrder, bool>> CreateFilter(GetMaxPurchaseOrderQuery request)
        {
            return e => e.TypeId == request.TypeId;
        }

        public override Expression<Func<PurchaseOrder, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
