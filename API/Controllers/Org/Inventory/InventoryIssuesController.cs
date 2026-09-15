using Inventory.Application.InventoryIssues.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InventoryIssuesController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public Task<Result<long>> Create(CreateInventoryIssueCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpPost("{id}/Lines")]
        public Task<Result> AddLine(long id, [FromBody] InventoryIssueLineInput line, CancellationToken cancellationToken) =>
            sender.Send(new AddInventoryIssueLineCommand(id, line.ProductId, line.UnitId, line.Quantity, line.BatchId, line.SerialId, line.Notes), cancellationToken);

        [HttpPut("{id}/Confirm")]
        public Task<Result> Confirm(long id, CancellationToken cancellationToken) =>
            sender.Send(new ConfirmInventoryIssueCommand(id), cancellationToken);

        [HttpPut("{id}/Post")]
        public Task<Result> Post(long id, CancellationToken cancellationToken) =>
            sender.Send(new PostInventoryIssueCommand(id), cancellationToken);

        [HttpPut("{id}/Cancel")]
        public Task<Result> Cancel(long id, CancellationToken cancellationToken) =>
            sender.Send(new CancelInventoryIssueCommand(id), cancellationToken);
    }
}
