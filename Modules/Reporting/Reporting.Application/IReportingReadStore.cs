namespace Reporting.Application;

using OrgSys.SharedKernel;
using Reporting.Application.Dealer.Queries;
using Reporting.Application.Financial.Queries;
using Reporting.Application.Sales.Queries;
using Reporting.Application.Warehouse.Queries;

/// <summary>
/// Cross-module SQL/read facade. Reporting.Application must not reference other modules' Domain;
/// the store is implemented in Reporting.Infrastructure against the shared repositories.
/// </summary>
public interface IReportingReadStore
{
    Task<ResultPagination<DealerBalance>> GetDealerBalanceAsync(GetDealerBalanceReportQuery request, CancellationToken cancellationToken);
    Task<ResultPagination<DealerStatment>> GetDealerStatementAsync(GetDealerStatementReportQuery request, CancellationToken cancellationToken);
    Task<ResultPagination<SalesBalance>> GetSalesBalanceAsync(GetSalesBalanceReportQuery request, CancellationToken cancellationToken);
    Task<ResultPagination<SafeBalance>> GetSafeBalanceAsync(GetSafeBalanceReportQuery request, CancellationToken cancellationToken);
    Task<ResultPagination<SafeStatment>> GetSafeMovementAsync(GetSafeMovementReportQuery request, CancellationToken cancellationToken);
    Task<ResultPagination<StockBalance>> GetWarehouseBalanceAsync(GetBalanceReportQuery request, CancellationToken cancellationToken);
    Task<ResultPagination<ProductStatment>> GetWarehouseMovementAsync(GetMovementReportQuery request, CancellationToken cancellationToken);
}
