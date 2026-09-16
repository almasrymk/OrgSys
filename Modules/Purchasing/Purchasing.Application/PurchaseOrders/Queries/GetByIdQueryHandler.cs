namespace Purchasing.Application.PurchaseOrders.Queries
{
    using AutoMapper;
    using Catalog.Contracts.Products;
    using Catalog.Contracts.Units;
    using MediatR;
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
    using System.Net;

    public sealed record GetByIdPurchaseOrderQuery(long Id) : ICommand<PurchaseOrderDto>, IGetByIdQuery<Result<PurchaseOrderDto>>;

    /// <summary>
    /// Bespoke handler replacing the generic OrgSys.SharedKernel.GetCommandHandler&lt;,,&gt; — that
    /// base requires TResponse : BaseModel, which PurchaseOrderDto no longer satisfies now that it
    /// is a standalone class instead of inheriting Purchasing.Domain.PurchaseOrder (see
    /// PurchaseOrderDto's own remark). Filter/include logic is unchanged except Dealer was dropped.
    /// </summary>
    public sealed class GetByIdQueryHandler(IRepository<PurchaseOrder> repository, IMapper mapper, ISender sender)
        : ICommandHandler<GetByIdPurchaseOrderQuery, PurchaseOrderDto>
    {
        public async Task<Result<PurchaseOrderDto>> Handle(GetByIdPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await repository.GetByFilterAsync(
                    e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true,
                    "PurchaseOrderProducts");

                var dto = order is null ? new PurchaseOrderDto() : mapper.Map<PurchaseOrderDto>(order);
                if (order is not null)
                {
                    var names = (await sender.Send(new GetDealerNamesQuery([order.DealerId]), cancellationToken)).Response ?? [];
                    dto.DealerName = names.GetValueOrDefault(order.DealerId);

                    var lines = dto.PurchaseOrderProductList;
                    if (lines is { Count: > 0 })
                    {
                        var productIds = lines.Select(e => e.ProductId).Distinct().ToList();
                        var unitIds = lines.Select(e => e.UnitId).Distinct().ToList();
                        var productNames = (await sender.Send(new GetProductNamesQuery(productIds), cancellationToken)).Response ?? [];
                        var unitNames = (await sender.Send(new GetUnitNamesQuery(unitIds), cancellationToken)).Response ?? [];
                        foreach (var line in lines)
                        {
                            line.ProductName = productNames.GetValueOrDefault(line.ProductId);
                            line.UnitName = unitNames.GetValueOrDefault(line.UnitId);
                        }
                    }
                }
                return new Result<PurchaseOrderDto>(HttpStatusCode.OK, dto, null);
            }
            catch (Exception ex)
            {
                return new Result<PurchaseOrderDto>(HttpStatusCode.InternalServerError, null, [new Error(ex.Message)]);
            }
        }
    }
}
