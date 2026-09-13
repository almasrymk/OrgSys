namespace Reporting.Application.Dealer.Queries
{
    using OrgSys.SharedKernel;
    using Treasury.Domain;
    using Domain.Enums;
    using System.Net;

    /// <summary>
    /// Backs both "Suppliers Statement" (DealerTypeId=2) and "Clients Statement" (DealerTypeId=1) —
    /// MVC's PurchasesController.SuppliersStatment / SalesController.ClientsStatment, both calling the
    /// legacy SalesReportService.GetDealersStatment(dealerTypeId, ...). Unlike Balance, this returns a
    /// dated row per Invoice/Financial in [FromDate, ToDate] plus a synthetic opening-balance row, with
    /// a running `Balance` column — the legacy SQL computed this via a window function; here it's an
    /// ordered in-memory cumulative sum per dealer (matches the codebase's existing "balance as of
    /// date" pattern, see GetDealerBalanceReportQueryHandler / GetListByBalanceQueryHandler).
    /// The legacy SQL's opening-balance sub-selects had `1 = 2` hardcoded (always empty) — a dead/
    /// never-finished leftover, not ported here; the opening balance below is a real running total of
    /// everything strictly before FromDate.
    /// </summary>
    public sealed record GetDealerStatementReportQuery(
        long DealerTypeId, DateTime FromDate, DateTime ToDate, long DealerId, long ShiftId, long BranchId, long UserId, int Page, int PageSize)
        : ICommandPagination<DealerStatment>;

    public sealed class GetDealerStatementReportQueryHandler(
        IRepository<Invoice> invoiceRepository,
        IRepository<Financial> financialRepository,
        IRepository<global::CommercialDocuments.Domain.InvoiceType> invoiceTypeRepository)
        : ICommandPaginationHandler<GetDealerStatementReportQuery, DealerStatment>
    {
        private sealed record Row(long DealerId, string? DealerName, string? DealerImgPath, DateTime Date, int Type, string? TypeName, string? Code, long? ReferenceId, decimal DisplayAmount, int DisplayInOut, decimal SignedAmount);

        public async Task<ResultPagination<DealerStatment>> Handle(GetDealerStatementReportQuery request, CancellationToken cancellationToken)
        {
            var invoiceTypes = (await invoiceTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
            var group = request.DealerTypeId == 1 ? "Sales" : "Purchases";
            var invoiceTypeIds = invoiceTypes.Where(e => e.Group == group).Select(e => e.Id).ToHashSet();
            var invoiceTypeById = invoiceTypes.ToDictionary(e => e.Id, e => e);

            var invoices = (await invoiceRepository.GetListByFilterAsync(
                e => invoiceTypeIds.Contains(e.TypeId)
                    && e.Dealer!.TypeId == request.DealerTypeId
                    && (request.DealerId == 0 || e.DealerId == request.DealerId)
                    && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                    && (request.BranchId == 0 || e.BranchId == request.BranchId)
                    && (request.UserId == 0 || e.CreateUserId == request.UserId)
                    && e.Status != Status.Deleted && !e.Hide,
                "Dealer"))?.ToList() ?? [];

            var financials = (await financialRepository.GetListByFilterAsync(
                e => e.Dealer != null && e.Dealer.TypeId == request.DealerTypeId
                    && (request.DealerId == 0 || e.DealerId == request.DealerId)
                    && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                    && (request.BranchId == 0 || e.BranchId == request.BranchId)
                    && (request.UserId == 0 || e.CreateUserId == request.UserId)
                    && e.Status != Status.Deleted && !e.Hide,
                "Dealer,FinancialType"))?.ToList() ?? [];

            var invoiceRows = invoices.Select(e =>
            {
                var invoiceType = invoiceTypeById.GetValueOrDefault(e.TypeId);
                return new Row(e.DealerId, e.Dealer?.Name, e.Dealer?.ImgPath, e.Date, 1,
                    invoiceType != null ? $"{invoiceType.Group} {invoiceType.Name}" : null,
                    e.Code, e.Id, e.NetByDefaultCurrency, invoiceType?.InOut ?? 0, e.NetByDefaultCurrency * (invoiceType?.InOut ?? 0));
            });

            var financialRows = financials.Where(e => e.DealerId.HasValue).Select(e =>
            {
                var inOut = e.FinancialType?.InOut ?? 0;
                return new Row(e.DealerId!.Value, e.Dealer?.Name, e.Dealer?.ImgPath, e.Date, 2,
                    e.FinancialType?.Name, e.Code, e.Id, e.AmountByDefaultCurrency, -1, -Math.Abs(e.AmountByDefaultCurrency * inOut));
            });

            var allRows = invoiceRows.Concat(financialRows).ToList();

            var opening = allRows
                .Where(r => r.Date < request.FromDate)
                .GroupBy(r => new { r.DealerId, r.DealerName, r.DealerImgPath })
                .ToDictionary(g => g.Key.DealerId, g => (g.Key.DealerName, g.Key.DealerImgPath, Sum: g.Sum(r => r.SignedAmount)));

            var inRange = allRows.Where(r => r.Date >= request.FromDate && r.Date <= request.ToDate).ToList();

            var dealerIds = opening.Keys.Union(inRange.Select(r => r.DealerId)).Distinct().ToList();

            var statements = new List<DealerStatment>();
            long syntheticId = 1;
            foreach (var dealerId in dealerIds)
            {
                var (name, imgPath, openingBalance) = opening.TryGetValue(dealerId, out var o)
                    ? o
                    : (inRange.First(r => r.DealerId == dealerId).DealerName, inRange.First(r => r.DealerId == dealerId).DealerImgPath, 0m);

                var runningBalance = openingBalance;
                statements.Add(new DealerStatment
                {
                    Id = syntheticId++,
                    ReferenceId = null,
                    Code = null,
                    OpenningBalance = 1,
                    Type = 0,
                    TypeName = "Opening Balance",
                    Date = request.FromDate,
                    DealerId = dealerId,
                    DealerName = name,
                    DealerImgPath = imgPath,
                    Amount = openingBalance,
                    InOut = 1,
                    Balance = runningBalance,
                });

                foreach (var row in inRange.Where(r => r.DealerId == dealerId).OrderBy(r => r.Date))
                {
                    runningBalance += row.SignedAmount;
                    statements.Add(new DealerStatment
                    {
                        Id = syntheticId++,
                        ReferenceId = row.ReferenceId,
                        Code = row.Code,
                        OpenningBalance = 0,
                        Type = row.Type,
                        TypeName = row.TypeName,
                        Date = row.Date,
                        DealerId = dealerId,
                        DealerName = row.DealerName,
                        DealerImgPath = row.DealerImgPath,
                        Amount = row.DisplayAmount,
                        InOut = row.DisplayInOut,
                        Balance = runningBalance,
                    });
                }
            }

            var ordered = statements.OrderBy(e => e.DealerId).ThenByDescending(e => e.OpenningBalance).ThenBy(e => e.Date).ToList();
            var page = ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
            var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
            return new ResultPagination<DealerStatment>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
        }
    }
}
