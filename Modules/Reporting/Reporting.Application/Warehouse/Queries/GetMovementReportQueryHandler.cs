namespace Reporting.Application.Warehouse.Queries;

using OrgSys.SharedKernel;

/// <summary>
/// Backs both "Stock Movement" and "Product Movement".
/// </summary>
public sealed record GetMovementReportQuery(DateTime FromDate, DateTime ToDate, long StockId, long ProductId, int Page, int PageSize)
    : ICommandPagination<ProductStatment>;

public sealed class GetMovementReportQueryHandler(IReportingReadStore store)
    : ICommandPaginationHandler<GetMovementReportQuery, ProductStatment>
{
    public Task<ResultPagination<ProductStatment>> Handle(GetMovementReportQuery request, CancellationToken cancellationToken)
        => store.GetWarehouseMovementAsync(request, cancellationToken);
}
