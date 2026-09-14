namespace Purchasing.Application.PurchaseOrders.Queries
{
    using AutoMapper;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record GetByIdPurchaseOrderQuery(long Id) : ICommand<PurchaseOrderDto>, IGetByIdQuery<Result<PurchaseOrderDto>>;

    /// <summary>
    /// Bespoke handler replacing the generic OrgSys.SharedKernel.GetCommandHandler&lt;,,&gt; — that
    /// base requires TResponse : BaseModel, which PurchaseOrderDto no longer satisfies now that it
    /// is a standalone class instead of inheriting Purchasing.Domain.PurchaseOrder (see
    /// PurchaseOrderDto's own remark). Filter/include logic is unchanged.
    /// </summary>
    public sealed class GetByIdQueryHandler(IRepository<PurchaseOrder> repository, IMapper mapper)
        : ICommandHandler<GetByIdPurchaseOrderQuery, PurchaseOrderDto>
    {
        public async Task<Result<PurchaseOrderDto>> Handle(GetByIdPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await repository.GetByFilterAsync(
                    e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true,
                    "Dealer,PurchaseOrderProducts,PurchaseOrderProducts.Unit");

                var dto = order is null ? new PurchaseOrderDto() : mapper.Map<PurchaseOrderDto>(order);
                return new Result<PurchaseOrderDto>(HttpStatusCode.OK, dto, null);
            }
            catch (Exception ex)
            {
                return new Result<PurchaseOrderDto>(HttpStatusCode.InternalServerError, null, [new Error(ex.Message)]);
            }
        }
    }
}
