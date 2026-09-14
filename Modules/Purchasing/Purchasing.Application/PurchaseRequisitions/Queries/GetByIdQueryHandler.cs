namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using AutoMapper;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record GetByIdPurchaseRequisitionQuery(long Id) : ICommand<PurchaseRequisitionDto>, IGetByIdQuery<Result<PurchaseRequisitionDto>>;

    /// <summary>Bespoke handler — see Purchasing.Application.PurchaseOrders.Queries.GetByIdQueryHandler's
    /// remark on why the generic OrgSys.SharedKernel.GetCommandHandler&lt;,,&gt; no longer applies.
    /// Filter/include logic unchanged.</summary>
    public sealed class GetByIdQueryHandler(IRepository<PurchaseRequisition> repository, IMapper mapper)
        : ICommandHandler<GetByIdPurchaseRequisitionQuery, PurchaseRequisitionDto>
    {
        public async Task<Result<PurchaseRequisitionDto>> Handle(GetByIdPurchaseRequisitionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var requisition = await repository.GetByFilterAsync(
                    e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true,
                    "PurchaseRequisitionProducts,PurchaseRequisitionProducts.Unit");

                var dto = requisition is null ? new PurchaseRequisitionDto() : mapper.Map<PurchaseRequisitionDto>(requisition);
                return new Result<PurchaseRequisitionDto>(HttpStatusCode.OK, dto, null);
            }
            catch (Exception ex)
            {
                return new Result<PurchaseRequisitionDto>(HttpStatusCode.InternalServerError, null, [new Error(ex.Message)]);
            }
        }
    }
}
