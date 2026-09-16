namespace Sales.Application.SalesOrders.Queries;

using System.Net;

public sealed record GetSalesOrderListQuery(long? CustomerId, SalesOrderStatus? Status) : ICommandCollection<SalesOrderDto>;

public sealed class GetSalesOrderListQueryHandler(IRepository<SalesOrder> repository)
    : ICommandCollectionHandler<GetSalesOrderListQuery, SalesOrderDto>
{
    public async Task<ResultCollection<SalesOrderDto>> Handle(GetSalesOrderListQuery request, CancellationToken cancellationToken)
    {
        var rows = (await repository.GetListByFilterAsync(e =>
            e.Status != Status.Deleted
            && (request.CustomerId == null || e.CustomerId == request.CustomerId)
            && (request.Status == null || e.LifecycleStatus == request.Status),
            "Lines"))?.OrderByDescending(e => e.Id).ToList() ?? [];

        return new ResultCollection<SalesOrderDto>(HttpStatusCode.OK, rows.Select(GetSalesOrderByIdQueryHandler.ToDto).ToList(), null);
    }
}
