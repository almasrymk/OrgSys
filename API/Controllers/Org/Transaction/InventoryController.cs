using Application.Commands.Org.Transaction.Inventory.Queries;
using Application.Commands.Org.Transactions.Inventory.Commands;
using Application.Commands.Org.Transactions.Inventory.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Shared;

namespace API.Controllers.Org.Transaction
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryController(ISender sender) : BaseController<GetByIdInventoryQuery ,SearchInventoryQuery, GetListInventoryQuery,CreateInventoryCommand,UpdateInventoryCommand,DeleteInventoryCommand, DeleteListInventoryCommand, GetMaxInventoryQuery,InventoryDto>(sender)
    {
        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new CancelInventoryCommand(Id), cancellationToken);
        }

        [HttpPut("Redo")]
        public async Task<Result> Redo(long Id, CancellationToken cancellationToken)
        {
            return await sender.Send(new RedoInventoryCommand(Id), cancellationToken);
        }

        [HttpPost("CreateAdjustment")]
        public Task<Result> CreateAdjustment(long InventoryId, CancellationToken cancellationToken)
        {
            return sender.Send(new CreateAdjustmentCommand(InventoryId), cancellationToken);
        }
    }
}
