using Inventory.Application.InventorySerials.Commands;
using Inventory.Application.InventorySerials.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InventorySerialsController(ISender sender) : ControllerBase
    {
        [HttpPost("Assign")]
        public Task<Result<long>> Assign(AssignSerialNumberCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpPut("{id}/Returned")]
        public Task<Result> MarkReturned(long id, long stockId, long? locationId, CancellationToken cancellationToken) =>
            sender.Send(new MarkSerialReturnedCommand(id, stockId, locationId), cancellationToken);

        [HttpPut("{id}/Damaged")]
        public Task<Result> MarkDamaged(long id, CancellationToken cancellationToken) =>
            sender.Send(new MarkSerialDamagedCommand(id), cancellationToken);

        [HttpPut("{id}/Lost")]
        public Task<Result> MarkLost(long id, CancellationToken cancellationToken) =>
            sender.Send(new MarkSerialLostCommand(id), cancellationToken);

        [HttpGet("History")]
        public Task<ResultCollection<SerialHistoryDto>> GetHistory(long productId, string? serialNumber, CancellationToken cancellationToken) =>
            sender.Send(new GetSerialHistoryQuery(productId, serialNumber), cancellationToken);
    }
}
