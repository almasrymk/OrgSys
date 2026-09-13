using Treasury.Application.FinancialTransfers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials;

[Route("[controller]")]
[ApiController]
public sealed class FinancialTransferController(ISender sender) : ControllerBase
{
    [HttpGet("GetList")]
    public Task<ResultCollection<FinancialTransferDto>> GetList(CancellationToken cancellationToken) =>
        sender.Send(new GetFinancialTransfersQuery(), cancellationToken);

    [HttpGet("GetById")]
    public Task<Result<FinancialTransferDto>> GetById(long id, CancellationToken cancellationToken) =>
        sender.Send(new GetFinancialTransferQuery(id), cancellationToken);

    [HttpPost("Post")]
    public Task<Result> Post([FromBody] FinancialTransferDto transfer, CancellationToken cancellationToken) =>
        sender.Send(new PostFinancialTransferCommand(transfer), cancellationToken);

    [HttpPut("Reverse")]
    public Task<Result> Reverse(long id, CancellationToken cancellationToken) =>
        sender.Send(new ReverseFinancialTransferCommand(id), cancellationToken);
}
