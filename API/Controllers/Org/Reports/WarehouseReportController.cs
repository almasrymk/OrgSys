using Reporting.Application.Warehouse.Queries;
using Reporting.Application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Reports
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WarehouseReportController(ISender sender) : ControllerBase
    {
        [HttpGet("Movement")]
        public async Task<ResultPagination<ProductStatment>> Movement(
            DateTime FromDate, DateTime ToDate, long StockId, long ProductId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetMovementReportQuery(FromDate, ToDate, StockId, ProductId, Page, PageSize), cancellationToken);
        }

        [HttpGet("Balance")]
        public async Task<ResultPagination<StockBalance>> Balance(
            DateTime ToDate, long ProductId, long StockId, long ClassificationId, bool SortByStock, int Page, int PageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetBalanceReportQuery(ToDate, ProductId, StockId, ClassificationId, SortByStock, Page, PageSize), cancellationToken);
        }
    }
}
