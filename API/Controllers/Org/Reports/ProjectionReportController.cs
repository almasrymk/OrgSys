using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reporting.Contracts.Projections;

namespace API.Controllers.Org.Reports;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ProjectionReportController(ISender sender) : ControllerBase
{
    [HttpGet("Aging")]
    public Task<Result<IReadOnlyList<CustomerAgingRowDto>>> Aging(long? customerId, DateTime asOfDate, CancellationToken cancellationToken) =>
        sender.Send(new GetCustomerAgingProjectionQuery(customerId, asOfDate), cancellationToken);

    [HttpGet("SalesSummary")]
    public Task<Result<IReadOnlyList<SalesSummaryRowDto>>> SalesSummary(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken) =>
        sender.Send(new GetSalesSummaryProjectionQuery(fromDate, toDate), cancellationToken);
}
