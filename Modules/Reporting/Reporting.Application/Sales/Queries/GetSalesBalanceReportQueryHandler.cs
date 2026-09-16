namespace Reporting.Application.Sales.Queries;

using OrgSys.SharedKernel;

/// <summary>
/// Backs "Sales Balance" — a daily Sales-invoice summary.
/// </summary>
public sealed record GetSalesBalanceReportQuery(DateTime FromDate, DateTime ToDate, int Page, int PageSize)
    : ICommandPagination<SalesBalance>;

public sealed class GetSalesBalanceReportQueryHandler(IReportingReadStore store)
    : ICommandPaginationHandler<GetSalesBalanceReportQuery, SalesBalance>
{
    public Task<ResultPagination<SalesBalance>> Handle(GetSalesBalanceReportQuery request, CancellationToken cancellationToken)
        => store.GetSalesBalanceAsync(request, cancellationToken);
}
