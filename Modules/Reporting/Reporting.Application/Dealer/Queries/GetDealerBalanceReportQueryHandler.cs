namespace Reporting.Application.Dealer.Queries;

using OrgSys.SharedKernel;

/// <summary>
/// Backs both "Suppliers Balance" (DealerTypeId=2) and "Clients Balance" (DealerTypeId=1).
/// </summary>
public sealed record GetDealerBalanceReportQuery(
    long DealerTypeId, DateTime ToDate, long DealerId, long ShiftId, long BranchId, long UserId, int Page, int PageSize)
    : ICommandPagination<DealerBalance>;

public sealed class GetDealerBalanceReportQueryHandler(IReportingReadStore store)
    : ICommandPaginationHandler<GetDealerBalanceReportQuery, DealerBalance>
{
    public Task<ResultPagination<DealerBalance>> Handle(GetDealerBalanceReportQuery request, CancellationToken cancellationToken)
        => store.GetDealerBalanceAsync(request, cancellationToken);
}
