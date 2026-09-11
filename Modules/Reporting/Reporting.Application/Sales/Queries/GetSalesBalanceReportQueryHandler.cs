namespace Reporting.Application.Sales.Queries
{
    using OrgSys.SharedKernel;
    using Domain.Enums;
    using System.Net;

    /// <summary>
    /// Backs "Sales Balance" (MVC's SalesController.SalesBalance / legacy SalesReportService.
    /// GetSalesBalance) — a daily Sales-invoice summary, not a per-dealer balance despite the name.
    /// Grouped by calendar date (the legacy SQL grouped by the raw Date column with no truncation,
    /// which would fragment into many rows per day if Date carries a time component — truncated to
    /// `.Date` here, almost certainly the original intent). `Net` only sums InvoiceType Id 1 (Sales
    /// Invoice), not Id 3 (Sales Return) — a legacy quirk, ported as-is rather than "fixed", since the
    /// original SQL is explicit about it (see docs/ANGULAR_MIGRATION_INVENTORY.md for the research
    /// this was ported from). A `FromDate` lower bound was added (the legacy report had none, `Date &lt;
    /// ToDate` only, which could return unbounded history).
    /// </summary>
    public sealed record GetSalesBalanceReportQuery(DateTime FromDate, DateTime ToDate, int Page, int PageSize)
        : ICommandPagination<SalesBalance>;

    public sealed class GetSalesBalanceReportQueryHandler(IRepository<Invoice> invoiceRepository)
        : ICommandPaginationHandler<GetSalesBalanceReportQuery, SalesBalance>
    {
        public async Task<ResultPagination<SalesBalance>> Handle(GetSalesBalanceReportQuery request, CancellationToken cancellationToken)
        {
            var invoices = (await invoiceRepository.GetListByFilterAsync(
                e => e.Date >= request.FromDate && e.Date < request.ToDate
                    && (e.TypeId == 1 || e.TypeId == 3)
                    && e.Status != Status.Deleted && !e.Hide,
                string.Empty))?.ToList() ?? [];

            var grouped = invoices
                .GroupBy(e => e.Date.Date)
                .Select(g => new SalesBalance
                {
                    Date = g.Key,
                    InAmount = g.Where(e => e.TypeId == 1).Sum(e => e.NetByDefaultCurrency),
                    OutAmount = g.Where(e => e.TypeId == 3).Sum(e => e.NetByDefaultCurrency),
                    Net = g.Where(e => e.TypeId == 1).Sum(e => e.NetByDefaultCurrency),
                })
                .ToList();

            var ordered = grouped.OrderByDescending(e => e.Date).ToList();
            var page = ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
            var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
            return new ResultPagination<SalesBalance>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
        }
    }
}
