namespace Reporting.Application.Dealer.Queries;

using OrgSys.SharedKernel;

/// <summary>
/// Backs both "Suppliers Statement" (DealerTypeId=2) and "Clients Statement" (DealerTypeId=1).
/// </summary>
public sealed record GetDealerStatementReportQuery(
    long DealerTypeId, DateTime FromDate, DateTime ToDate, long DealerId, long ShiftId, long BranchId, long UserId, int Page, int PageSize)
    : ICommandPagination<DealerStatment>;

public sealed class GetDealerStatementReportQueryHandler(IReportingReadStore store)
    : ICommandPaginationHandler<GetDealerStatementReportQuery, DealerStatment>
{
    public Task<ResultPagination<DealerStatment>> Handle(GetDealerStatementReportQuery request, CancellationToken cancellationToken)
        => store.GetDealerStatementAsync(request, cancellationToken);
}
