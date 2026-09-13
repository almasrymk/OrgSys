namespace Reporting.Application.Warehouse.Queries
{
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>
    /// Backs both "Stock Movement" and "Product Movement" (MVC's WarehousesController.StockMovement/
    /// ProductMovement) with one query — the legacy StockStatmentSql had its filter clause dead/commented
    /// out, ProductStatmentSql is the working twin of the same query; this ports that working logic once
    /// and returns the richer ProductStatment shape (StockStatment is a strict subset of the same fields).
    /// </summary>
    public sealed record GetMovementReportQuery(DateTime FromDate, DateTime ToDate, long StockId, long ProductId, int Page, int PageSize)
        : ICommandPagination<ProductStatment>;

    public sealed class GetMovementReportQueryHandler(
        IRepository<TransactionProduct> transactionProductRepository,
        IRepository<Inventory.Domain.TransactionType> transactionTypeRepository)
        : ICommandPaginationHandler<GetMovementReportQuery, ProductStatment>
    {
        public async Task<ResultPagination<ProductStatment>> Handle(GetMovementReportQuery request, CancellationToken cancellationToken)
        {
            var types = (await transactionTypeRepository.GetListByFilterAsync(e => true, string.Empty))?.ToList() ?? [];
            var typeNames = types.ToDictionary(e => e.Id, e => e.Name);

            var items = (await transactionProductRepository.GetListByFilterAsync(
                e => e.Transaction!.Date >= request.FromDate && e.Transaction.Date <= request.ToDate
                    && (request.StockId == 0 || e.StockId == request.StockId)
                    && (request.ProductId == 0 || e.ProductId == request.ProductId)
                    && e.Transaction.Status != Status.Deleted && !e.Transaction.Hide,
                "Transaction,Product,Product.Classification,Stock"))?.ToList() ?? [];

            var ordered = items
                .OrderByDescending(e => e.TransactionId)
                .ThenBy(e => e.ProductId)
                .ToList();

            var page = ordered
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new ProductStatment
                {
                    Id = e.Id,
                    ReferenceId = e.TransactionId,
                    ProductCode = e.Product?.Code,
                    TransactionCode = e.Transaction?.Code,
                    TypeId = e.Transaction?.TypeId,
                    TypeName = e.Transaction != null && typeNames.TryGetValue(e.Transaction.TypeId, out var name) ? name : null,
                    Date = e.Transaction?.Date ?? default,
                    ProductId = e.ProductId,
                    ProductName = e.Product?.Name,
                    StockId = e.StockId ?? 0,
                    StockName = e.Stock?.Name,
                    Quantity = e.Quantity,
                    ProductImgPath = e.Product?.ImgPath,
                    ClassificationId = e.Product?.ClassificationId ?? 0,
                    ClassificationName = e.Product?.Classification?.Name,
                })
                .ToList();

            var pageCount = request.PageSize > 0 ? (int)Math.Ceiling(ordered.Count / (double)request.PageSize) : 0;
            return new ResultPagination<ProductStatment>(HttpStatusCode.OK, page, request.Page, request.PageSize, pageCount, null);
        }
    }
}
