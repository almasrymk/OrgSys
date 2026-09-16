namespace Purchasing.Application.PurchaseOrders.Queries;

using Purchasing.Contracts.PurchaseOrders;
using System.Net;

public sealed class GetPurchaseOrderRemainingQueryHandler(IRepository<PurchaseOrder> repository)
    : IQueryHandler<GetPurchaseOrderRemainingQuery, PurchaseOrderRemainingDto?>
{
    public async Task<Result<PurchaseOrderRemainingDto?>> Handle(GetPurchaseOrderRemainingQuery request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByFilterAsync(
            e => e.Id == request.PurchaseOrderId && e.Status != Status.Deleted,
            "PurchaseOrderProducts");
        if (order is null)
            return new Result<PurchaseOrderRemainingDto?>(HttpStatusCode.OK, null, null);

        var dto = new PurchaseOrderRemainingDto(
            order.Id,
            order.Code,
            order.DealerId,
            order.PurchaseOrderProducts.Select(l => new PurchaseOrderRemainingLineDto(
                l.Id, l.ProductId, l.UnitId, l.Quantity, l.RemainingQuantity)).ToList());

        return new Result<PurchaseOrderRemainingDto?>(HttpStatusCode.OK, dto, null);
    }
}
