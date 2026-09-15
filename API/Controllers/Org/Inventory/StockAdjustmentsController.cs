using Inventory.Application.StockAdjustments.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StockAdjustmentsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public Task<Result<long>> Create(CreateStockAdjustmentCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpPut("{id}/Post")]
        public Task<Result> Post(long id, CancellationToken cancellationToken) =>
            sender.Send(new PostStockAdjustmentCommand(id), cancellationToken);

        [HttpPut("{id}/Cancel")]
        public Task<Result> Cancel(long id, CancellationToken cancellationToken) =>
            sender.Send(new CancelStockAdjustmentCommand(id), cancellationToken);
    }
}
