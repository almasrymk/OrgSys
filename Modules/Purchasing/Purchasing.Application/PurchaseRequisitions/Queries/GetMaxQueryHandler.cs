namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record GetMaxPurchaseRequisitionQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<PurchaseRequisition> _Repository)
        : GetMaxCommandHandler<GetMaxPurchaseRequisitionQuery, PurchaseRequisition>(_Repository)
    {
        public override Expression<Func<PurchaseRequisition, bool>> CreateFilter(GetMaxPurchaseRequisitionQuery request)
        {
            return e => e.TypeId == request.TypeId;
        }

        public override Expression<Func<PurchaseRequisition, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
