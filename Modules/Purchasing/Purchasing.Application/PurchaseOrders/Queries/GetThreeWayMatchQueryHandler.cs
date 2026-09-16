namespace Purchasing.Application.PurchaseOrders.Queries;

using CommercialDocuments.Contracts.Invoices;
using MediatR;
using Purchasing.Contracts.PurchaseOrders;
using System.Net;

public sealed class GetThreeWayMatchQueryHandler(IRepository<PurchaseOrder> repository, ISender sender)
    : IQueryHandler<GetThreeWayMatchQuery, ThreeWayMatchDto?>
{
    public async Task<Result<ThreeWayMatchDto?>> Handle(GetThreeWayMatchQuery request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByFilterAsync(
            e => e.Id == request.PurchaseOrderId && e.Status != Status.Deleted,
            "PurchaseOrderProducts");
        if (order is null)
            return new Result<ThreeWayMatchDto?>(HttpStatusCode.OK, null, null);

        var invoicedByProduct = new Dictionary<(long ProductId, long UnitId), decimal>();
        if (order.InvoiceId is > 0)
        {
            var impact = (await sender.Send(new GetInvoiceInventoryImpactQuery(order.InvoiceId.Value), cancellationToken)).Response;
            if (impact?.Lines is not null)
            {
                foreach (var line in impact.Lines)
                {
                    var key = (line.ProductId, line.UnitId);
                    invoicedByProduct[key] = invoicedByProduct.GetValueOrDefault(key) + line.Quantity;
                }
            }
        }

        var lines = order.PurchaseOrderProducts.Select(l =>
        {
            var invoiced = invoicedByProduct.GetValueOrDefault((l.ProductId, l.UnitId));
            return new ThreeWayMatchLineDto(
                l.Id,
                l.ProductId,
                l.UnitId,
                l.Quantity,
                l.ReceivedQuantity,
                invoiced,
                l.RemainingQuantity,
                l.Quantity - invoiced);
        }).ToList();

        return new Result<ThreeWayMatchDto?>(
            HttpStatusCode.OK,
            new ThreeWayMatchDto(order.Id, order.InvoiceId, lines),
            null);
    }
}
