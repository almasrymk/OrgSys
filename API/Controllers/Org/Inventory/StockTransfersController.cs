using Inventory.Application.StockTransfers.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StockTransfersController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public Task<Result<long>> Create(CreateStockTransferCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpPost("{id}/Lines")]
        public Task<Result> AddLine(long id, [FromBody] StockTransferLineInput line, CancellationToken cancellationToken) =>
            sender.Send(new AddStockTransferLineCommand(id, line.ProductId, line.UnitId, line.Quantity, line.BatchId, line.Notes), cancellationToken);

        [HttpPut("{id}/Confirm")]
        public Task<Result> Confirm(long id, CancellationToken cancellationToken) =>
            sender.Send(new ConfirmStockTransferCommand(id), cancellationToken);

        [HttpPut("{id}/Ship")]
        public Task<Result> Ship(long id, bool immediate, CancellationToken cancellationToken) =>
            sender.Send(new ShipStockTransferCommand(id, immediate), cancellationToken);

        [HttpPut("{id}/Receive")]
        public Task<Result> Receive(long id, CancellationToken cancellationToken) =>
            sender.Send(new ReceiveStockTransferCommand(id), cancellationToken);

        [HttpPut("{id}/Cancel")]
        public Task<Result> Cancel(long id, CancellationToken cancellationToken) =>
            sender.Send(new CancelStockTransferCommand(id), cancellationToken);
    }
}
