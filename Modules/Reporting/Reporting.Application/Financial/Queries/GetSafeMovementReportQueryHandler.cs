namespace Reporting.Application.Financial.Queries;

using OrgSys.SharedKernel;

/// <summary>
/// Backs "Safe Movement" — flat CashBox ledger movement list.
/// </summary>
public sealed record GetSafeMovementReportQuery(
    DateTime FromDate, DateTime ToDate, long DealerId, long CashBoxId, int Page, int PageSize)
    : ICommandPagination<SafeStatment>;

public sealed class GetSafeMovementReportQueryHandler(IReportingReadStore store)
    : ICommandPaginationHandler<GetSafeMovementReportQuery, SafeStatment>
{
    public Task<ResultPagination<SafeStatment>> Handle(GetSafeMovementReportQuery request, CancellationToken cancellationToken)
        => store.GetSafeMovementAsync(request, cancellationToken);
}
