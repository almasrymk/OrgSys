namespace Sales.Application.SalesOrders.Queries;

using System.Net;

public sealed record GetSalesOrderByIdQuery(long Id) : IQuery<SalesOrderDto>;

public sealed class GetSalesOrderByIdQueryHandler(IRepository<SalesOrder> repository)
    : IQueryHandler<GetSalesOrderByIdQuery, SalesOrderDto>
{
    public async Task<Result<SalesOrderDto>> Handle(GetSalesOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, "Lines");
        if (order is null)
            return new Result<SalesOrderDto>(HttpStatusCode.NotFound, null, [new Error("Sales order not found.")]);

        return new Result<SalesOrderDto>(HttpStatusCode.OK, ToDto(order), null);
    }

    internal static SalesOrderDto ToDto(SalesOrder order) => new(
        order.Id, order.Code, order.CustomerId, order.RequestedDeliveryDate, order.CurrencyId, order.Rate,
        order.SourceQuotationId, order.Subtotal, order.DiscountAmount, order.TaxAmount, order.TotalAmount,
        order.LifecycleStatus, order.Notes, order.Date, order.BranchId,
        order.Lines.Select(l => new SalesOrderLineDto(
            l.Id, l.ProductId, l.ProductName, l.UnitId, l.OrderedQuantity, l.UnitPrice, l.DiscountAmount, l.TaxAmount,
            l.LineTotal, l.DeliveredQuantity, l.ReturnedQuantity, l.CancelledQuantity, l.RequestedDeliveryDate, l.Notes)).ToList());
}
