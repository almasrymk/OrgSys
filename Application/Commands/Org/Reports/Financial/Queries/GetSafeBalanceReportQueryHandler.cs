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
    /// Backs "Safe Balance" (MVC's FinancesController.SafeBalance). In the legacy app this report was
    /// never actually finished/wired up (the controller action's real call was commented out, and its
    /// own DTO/SQL were inconsistent with each other) — this is a fresh, simplified implementation:
    /// one row per CashBox with its running balance as of `ToDate`, same signed-sum-as-of-date pattern
    /// used elsewhere in the Reports queries, rather than porting the legacy SQL's unfinished
    /// per-(CashBox,FinancialType,User,Shift) breakdown.
    /// </summary>
    public sealed record GetSafeBalanceReportQuery(DateTime ToDate, long CashBoxId, long UserId, long ShiftId, int Page, int PageSize)
        : ICommandPagination<SafeBalance>;

    public sealed class GetSafeBalanceReportQueryHandler(
        IRepository<Domain.Entities.Financial> financialRepository,
        IRepository<CashBox> cashBoxRepository,
        IRepository<Domain.Entities.FinancialType> financialTypeRepository)
        : ICommandPaginationHandler<GetSafeBalanceReportQuery, SafeBalance>
    {
        public async Task<ResultPagination<SafeBalance>> Handle(GetSafeBalanceReportQuery request, CancellationToken cancellationToken)
        {
            var cashBoxes = (await cashBoxRepository.GetListByFilterAsync(
                e => e.FinancialAccountId != null && (request.CashBoxId == 0 || e.Id == request.CashBoxId),
                string.Empty))?.ToList() ?? [];
            var cashBoxByFinancialAccountId = cashBoxes.Where(e => e.FinancialAccountId.HasValue).ToDictionary(e => e.FinancialAccountId!.Value, e => e);

            var financialTypes = (await financialTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
            var inOutByTypeId = financialTypes.ToDictionary(e => e.Id, e => e.InOut);

            var financials = (await financialRepository.GetListByFilterAsync(
                e => e.Date <= request.ToDate
                    && e.FinancialAccountId != null
                    && (request.UserId == 0 || e.CreateUserId == request.UserId)
                    && (request.ShiftId == 0 || e.ShiftId == request.ShiftId)
                    && e.Status != Status.Deleted && !e.Hide,
                string.Empty))?.ToList() ?? [];

            var relevant = financials.Where(e => cashBoxByFinancialAccountId.ContainsKey(e.FinancialAccountId!.Value)).ToList();

            var grouped = relevant
                .GroupBy(e => e.FinancialAccountId!.Value)
                .Select(g =>
                {
                    var cashBox = cashBoxByFinancialAccountId[g.Key];
                    return new SafeBalance
                    {
                        Id = cashBox.Id,
                        SafeId = cashBox.Id,
                        SafeName = cashBox.Name,
                        Balance = g.Sum(e => e.AmountByDefaultCurrency * (e.FinancialTypeId.HasValue && inOutByTypeId.TryGetValue(e.FinancialTypeId.Value, out var io) ? io : 0)),
                    };
                })
                .ToList();

            var ordered = grouped.OrderBy(e => e.SafeId).ToList();
            var page = ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
            var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
            return new ResultPagination<SafeBalance>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
        }
    }
}
