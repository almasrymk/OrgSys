namespace Sales.Application.SalesOrders.Commands;

using Inventory.Contracts.Availability;
using MediatR;
using System.Net;

public sealed record ConfirmSalesOrderCommand(long Id, long StockId, long? LocationId, long CreateUserId) : ICommand;

public sealed class ConfirmSalesOrderCommandHandler(
    IRepository<SalesOrder> repository,
    IUnitOfWork unitOfWork,
    ISender sender)
    : ICommandHandler<ConfirmSalesOrderCommand>
{
    public async Task<Result> Handle(ConfirmSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, "Lines");
        if (order is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Sales order not found.")]);

        if (order.LifecycleStatus != SalesOrderStatus.Draft || order.Lines.Count == 0)
            return new Result(HttpStatusCode.BadRequest, [new Error("Only a Draft sales order with lines can be confirmed.")]);

        if (request.StockId <= 0)
            return new Result(HttpStatusCode.BadRequest, [new Error("A stock is required to reserve inventory on confirm.")]);

        foreach (var line in order.Lines)
        {
            var reserved = await sender.Send(new ReserveInventoryCommand(
                line.ProductId, request.StockId, request.LocationId, null, line.OrderedQuantity,
                ReservationSourceType.SalesOrder, order.Id, line.Id, request.CreateUserId), cancellationToken);
            if (reserved.Response is not > 0)
                return new Result(reserved.StatusCode, reserved.Errors);
        }

        try
        {
            order.Confirm();
            await repository.UpdateAsync(order);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (SalesOrderDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}
