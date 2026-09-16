namespace Reporting.Application.Warehouse.Queries;

using OrgSys.SharedKernel;

/// <summary>
/// Backs both "Stock Balance" and "Product Balance".
/// </summary>
public sealed record GetBalanceReportQuery(
    DateTime ToDate, long ProductId, long StockId, long ClassificationId, bool SortByStock, int Page, int PageSize)
    : ICommandPagination<StockBalance>;

public sealed class GetBalanceReportQueryHandler(IReportingReadStore store)
    : ICommandPaginationHandler<GetBalanceReportQuery, StockBalance>
{
    public Task<ResultPagination<StockBalance>> Handle(GetBalanceReportQuery request, CancellationToken cancellationToken)
        => store.GetWarehouseBalanceAsync(request, cancellationToken);
}
