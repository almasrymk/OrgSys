namespace Reporting.Application.Warehouse.Queries
{
    using OrgSys.SharedKernel;
    using Domain.Enums;
    using System.Net;

    /// <summary>
    /// Backs both "Stock Balance" and "Product Balance" (MVC's WarehousesController.StockBalance/
    /// ProductBalance) with one query — the legacy SQL for both reports was byte-for-byte identical
    /// (same grouping, same filters), differing only in sort order/primary-key emphasis in the view.
    /// Same signed-sum-of-transactions "balance as of date" pattern as
    /// Application/Commands/Org/Setting/Product/Queries/GetListByBalanceQueryHandler.cs, generalized
    /// to group by (Stock, Product, Classification) and to use TransactionType.InOut instead of a
    /// hardcoded TypeId list.
    /// </summary>
    public sealed record GetBalanceReportQuery(
        DateTime ToDate, long ProductId, long StockId, long ClassificationId, bool SortByStock, int Page, int PageSize)
        : ICommandPagination<StockBalance>;

    public sealed class GetBalanceReportQueryHandler(
        IRepository<TransactionProduct> transactionProductRepository,
        IRepository<Inventory.Domain.TransactionType> transactionTypeRepository)
        : ICommandPaginationHandler<GetBalanceReportQuery, StockBalance>
    {
        public async Task<ResultPagination<StockBalance>> Handle(GetBalanceReportQuery request, CancellationToken cancellationToken)
        {
            var types = (await transactionTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
            var inOutByTypeId = types.ToDictionary(e => e.Id, e => e.InOut);

            var items = (await transactionProductRepository.GetListByFilterAsync(
                e => e.Transaction!.Date <= request.ToDate
                    && (request.ProductId == 0 || e.ProductId == request.ProductId)
                    && (request.StockId == 0 || e.StockId == request.StockId)
                    && (request.ClassificationId == 0 || e.Product!.ClassificationId == request.ClassificationId)
                    && e.Transaction.Status != Status.Deleted && !e.Transaction.Hide,
                "Transaction,Product,Product.Classification,Stock"))?.ToList() ?? [];

            var grouped = items
                .GroupBy(e => new
                {
                    StockId = e.StockId ?? 0,
                    StockName = e.Stock?.Name,
                    ProductId = e.ProductId,
                    ProductName = e.Product?.Name,
                    ClassificationId = e.Product?.ClassificationId ?? 0,
                    ClassificationName = e.Product?.Classification?.Name,
                })
                .Select(g => new StockBalance
                {
                    StockId = g.Key.StockId,
                    StockName = g.Key.StockName,
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    ClassificationId = g.Key.ClassificationId,
                    ClassificationName = g.Key.ClassificationName,
                    Balance = g.Sum(e => e.Quantity * (e.Transaction != null && inOutByTypeId.TryGetValue(e.Transaction.TypeId, out var inOut) ? inOut : 0)),
                })
                .ToList();

            var ordered = request.SortByStock
                ? grouped.OrderBy(e => e.StockId).ThenBy(e => e.ProductId).ToList()
                : grouped.OrderBy(e => e.ProductId).ThenBy(e => e.StockId).ToList();

            var page = ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
            var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
            return new ResultPagination<StockBalance>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
        }
    }
}
