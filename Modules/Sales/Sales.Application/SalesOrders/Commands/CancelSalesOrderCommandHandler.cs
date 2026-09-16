namespace Sales.Application.SalesOrders.Commands;

using System.Net;

public sealed record CancelSalesOrderCommand(long Id) : ICommand;

public sealed class CancelSalesOrderCommandHandler(IRepository<SalesOrder> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<CancelSalesOrderCommand>
{
    public async Task<Result> Handle(CancelSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, "Lines");
        if (order is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Sales order not found.")]);

        try
        {
            order.Cancel();
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
