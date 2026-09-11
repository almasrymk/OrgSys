using Reporting.Application.Dealer.Queries;
using Reporting.Application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Reports
{
    /// <summary>Powers both "Suppliers Balance/Statement" and "Clients Balance/Statement" — one query per shape, parameterized by DealerTypeId (1=Client, 2=Supplier).</summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DealerReportController(ISender sender) : ControllerBase
    {
        [HttpGet("Balance")]
        public async Task<ResultPagination<DealerBalance>> Balance(
            long DealerTypeId, DateTime ToDate, long DealerId, long ShiftId, long BranchId, long UserId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetDealerBalanceReportQuery(DealerTypeId, ToDate, DealerId, ShiftId, BranchId, UserId, Page, PageSize), cancellationToken);
        }

        [HttpGet("Statement")]
        public async Task<ResultPagination<DealerStatment>> Statement(
            long DealerTypeId, DateTime FromDate, DateTime ToDate, long DealerId, long ShiftId, long BranchId, long UserId, int Page, int PageSize, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetDealerStatementReportQuery(DealerTypeId, FromDate, ToDate, DealerId, ShiftId, BranchId, UserId, Page, PageSize), cancellationToken);
        }
    }
}
