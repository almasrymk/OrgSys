using Application.Commands.Org.Reports.Financial.Queries;
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
    public class FinancialReportController(ISender sender) : ControllerBase
    {
        [HttpGet("SafeMovement")]
        public async Task<ResultPagination<SafeStatment>> SafeMovement(
            DateTime FromDate, DateTime ToDate, long DealerId, long CashBoxId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetSafeMovementReportQuery(FromDate, ToDate, DealerId, CashBoxId, Page, PageSize), cancellationToken);
        }

        [HttpGet("SafeBalance")]
        public async Task<ResultPagination<SafeBalance>> SafeBalance(
            DateTime ToDate, long CashBoxId, long UserId, long ShiftId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetSafeBalanceReportQuery(ToDate, CashBoxId, UserId, ShiftId, Page, PageSize), cancellationToken);
        }
    }
}
