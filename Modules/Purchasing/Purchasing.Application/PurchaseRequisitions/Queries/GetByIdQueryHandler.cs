namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPurchaseRequisitionQuery(long Id) : ICommand<PurchaseRequisitionDto>, IGetByIdQuery<Result<PurchaseRequisitionDto>>;

    public sealed class GetByIdQueryHandler(IRepository<PurchaseRequisition> _Repository, IMapper mapper)
        : GetCommandHandler<GetByIdPurchaseRequisitionQuery, PurchaseRequisition, PurchaseRequisitionDto>(_Repository, mapper)
    {
        public override Expression<Func<PurchaseRequisition, bool>> CreateFilter(GetByIdPurchaseRequisitionQuery request)
        {
            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "PurchaseRequisitionProducts,PurchaseRequisitionProducts.Unit";
        }
    }
}
