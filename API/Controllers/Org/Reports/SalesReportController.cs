using Application.Commands.Org.Reports.Sales.Queries;
using Application.Report;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Reports
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SalesReportController(ISender sender) : ControllerBase
    {
        [HttpGet("Balance")]
        public async Task<ResultPagination<SalesBalance>> Balance(DateTime FromDate, DateTime ToDate, int Page, int PageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetSalesBalanceReportQuery(FromDate, ToDate, Page, PageSize), cancellationToken);
        }
    }
}
