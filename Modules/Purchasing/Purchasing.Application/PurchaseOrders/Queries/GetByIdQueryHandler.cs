namespace Purchasing.Application.PurchaseOrders.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPurchaseOrderQuery(long Id) : ICommand<PurchaseOrderDto>, IGetByIdQuery<Result<PurchaseOrderDto>>;

    public sealed class GetByIdQueryHandler(IRepository<PurchaseOrder> _Repository, IMapper mapper)
        : GetCommandHandler<GetByIdPurchaseOrderQuery, PurchaseOrder, PurchaseOrderDto>(_Repository, mapper)
    {
        public override Expression<Func<PurchaseOrder, bool>> CreateFilter(GetByIdPurchaseOrderQuery request)
        {
            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,PurchaseOrderProducts,PurchaseOrderProducts.Unit";
        }
    }
}
