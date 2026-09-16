using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tax.Contracts.Snapshots;

namespace API.Controllers.Org.Tax;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TaxController(ISender sender) : ControllerBase
{
    [HttpGet("Snapshot")]
    public Task<Result<InvoiceTaxSnapshotDto>> Snapshot(long invoiceId, CancellationToken cancellationToken) =>
        sender.Send(new GetInvoiceTaxSnapshotQuery(invoiceId), cancellationToken);
}
