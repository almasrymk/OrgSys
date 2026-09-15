using Inventory.Application.InventoryBatches.Commands;
using Inventory.Application.InventoryBatches.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InventoryBatchesController(ISender sender) : ControllerBase
    {
        [HttpPost("Assign")]
        public Task<Result<long>> Assign(AssignBatchCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpPut("{id}/Quarantine")]
        public Task<Result> Quarantine(long id, CancellationToken cancellationToken) =>
            sender.Send(new QuarantineBatchCommand(id), cancellationToken);

        [HttpPut("{id}/Activate")]
        public Task<Result> Activate(long id, CancellationToken cancellationToken) =>
            sender.Send(new ActivateBatchCommand(id), cancellationToken);

        [HttpGet("Expiring")]
        public Task<ResultCollection<BatchExpiryDto>> GetExpiring(long? productId, int withinDays, CancellationToken cancellationToken) =>
            sender.Send(new GetExpiringBatchesQuery(productId, withinDays), cancellationToken);
    }
}
