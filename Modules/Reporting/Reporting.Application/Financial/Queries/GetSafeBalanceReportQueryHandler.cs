namespace Reporting.Application.Financial.Queries;

using OrgSys.SharedKernel;

/// <summary>
/// Backs "Safe Balance" — one row per CashBox with running balance as of ToDate.
/// </summary>
public sealed record GetSafeBalanceReportQuery(DateTime ToDate, long CashBoxId, long UserId, long ShiftId, int Page, int PageSize)
    : ICommandPagination<SafeBalance>;

public sealed class GetSafeBalanceReportQueryHandler(IReportingReadStore store)
    : ICommandPaginationHandler<GetSafeBalanceReportQuery, SafeBalance>
{
    public Task<ResultPagination<SafeBalance>> Handle(GetSafeBalanceReportQuery request, CancellationToken cancellationToken)
        => store.GetSafeBalanceAsync(request, cancellationToken);
}
