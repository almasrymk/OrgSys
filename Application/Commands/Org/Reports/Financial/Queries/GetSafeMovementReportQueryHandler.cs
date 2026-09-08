namespace Application.Commands.Org.Reports.Financial.Queries
{
    using Application.Abstraction.Command;
    using Application.Report;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using System.Net;

    /// <summary>
    /// Backs "Safe Movement" (MVC's FinancesController.SafeMovement / legacy FinancesReportService.
    /// GetSafeStatment). The legacy "Safe" entity is now CashBox, which no longer holds its own ledger —
    /// it has a 1:1 FinancialAccountId, and Financial posts against FinancialAccountId directly (no more
    /// Financial.SafeId) — so "safe" rows are resolved as Financial rows whose FinancialAccountId
    /// matches the chosen CashBox's FinancialAccountId. Flat movement list, no running balance (see
    /// GetSafeBalanceReportQueryHandler for that).
    /// </summary>
    public sealed record GetSafeMovementReportQuery(
        DateTime FromDate, DateTime ToDate, long DealerId, long CashBoxId, int Page, int PageSize)
        : ICommandPagination<SafeStatment>;

    public sealed class GetSafeMovementReportQueryHandler(
        IRepository<Domain.Entities.Financial> financialRepository,
        IRepository<CashBox> cashBoxRepository,
        IRepository<Domain.Entities.FinancialType> financialTypeRepository)
        : ICommandPaginationHandler<GetSafeMovementReportQuery, SafeStatment>
    {
        public async Task<ResultPagination<SafeStatment>> Handle(GetSafeMovementReportQuery request, CancellationToken cancellationToken)
        {
            var cashBoxes = (await cashBoxRepository.GetListByFilterAsync(e => e.FinancialAccountId != null, string.Empty))?.ToList() ?? [];
            var cashBoxByFinancialAccountId = cashBoxes.Where(e => e.FinancialAccountId.HasValue).ToDictionary(e => e.FinancialAccountId!.Value, e => e);
            long? financialAccountId = request.CashBoxId != 0
                ? cashBoxes.FirstOrDefault(e => e.Id == request.CashBoxId)?.FinancialAccountId
                : null;

            var financialTypes = (await financialTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
            var typeNameById = financialTypes.ToDictionary(e => e.Id, e => e.Name);

            var financials = (await financialRepository.GetListByFilterAsync(
                e => e.Date >= request.FromDate && e.Date <= request.ToDate
                    && e.FinancialAccountId != null
                    && (request.DealerId == 0 || e.DealerId == request.DealerId)
                    && (financialAccountId == null || e.FinancialAccountId == financialAccountId)
                    && e.Status != Status.Deleted && !e.Hide,
                "Dealer,Currency"))?.ToList() ?? [];

            var relevant = financials.Where(e => cashBoxByFinancialAccountId.ContainsKey(e.FinancialAccountId!.Value)).ToList();

            var ordered = relevant.OrderByDescending(e => e.Id).ThenBy(e => e.FinancialAccountId).ToList();

            var page = ordered
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e =>
                {
                    var cashBox = cashBoxByFinancialAccountId[e.FinancialAccountId!.Value];
                    return new SafeStatment
                    {
                        Id = e.Id,
                        ReferenceId = e.Id,
                        Code = e.Code,
                        Amount = e.AmountByDefaultCurrency,
                        TypeId = e.FinancialTypeId,
                        TypeName = e.FinancialTypeId.HasValue && typeNameById.TryGetValue(e.FinancialTypeId.Value, out var name) ? name : null,
                        Date = e.Date,
                        SafeId = cashBox.Id,
                        SafeName = cashBox.Name,
                        DealerId = e.DealerId ?? 0,
                        DealerName = e.Dealer?.Name,
                        CurrencyId = e.CurrencyId,
                        CurrencyName = e.Currency?.Name,
                    };
                })
                .ToList();

            var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
            return new ResultPagination<SafeStatment>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
        }
    }
}
