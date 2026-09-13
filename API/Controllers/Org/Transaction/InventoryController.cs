using Inventory.Application.Inventories.Queries;
using Inventory.Application.Inventories.Commands;
using Inventory.Application.Inventories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Transaction
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryController(ISender sender) : BaseController<GetByIdInventoryQuery ,SearchInventoryQuery, GetListInventoryQuery,CreateInventoryCommand,UpdateInventoryCommand,DeleteInventoryCommand, DeleteListInventoryCommand, GetMaxInventoryQuery,InventoryDto>(sender)
    {
        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken)
        {
            return await Sender.Send(new CancelInventoryCommand(Id), cancellationToken);
        }

        [HttpPut("Redo")]
        public async Task<Result> Redo(long Id, CancellationToken cancellationToken)
        {
            return await Sender.Send(new RedoInventoryCommand(Id), cancellationToken);
        }

        [HttpPost("CreateAdjustment")]
        public Task<Result> CreateAdjustment(long InventoryId, CancellationToken cancellationToken)
        {
            return Sender.Send(new CreateAdjustmentCommand(InventoryId), cancellationToken);
        }
    }
}
