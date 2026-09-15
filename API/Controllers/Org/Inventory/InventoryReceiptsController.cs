using Inventory.Application.InventoryReceipts.Commands;
using Inventory.Application.InventoryReceipts.Queries;
using Inventory.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InventoryReceiptsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public Task<Result<long>> Create(CreateInventoryReceiptCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpGet("{id}")]
        public Task<Result<InventoryReceiptDto>> GetById(long id, CancellationToken cancellationToken) =>
            sender.Send(new GetInventoryReceiptByIdQuery(id), cancellationToken);

        [HttpGet]
        public Task<ResultCollection<InventoryReceiptDto>> GetList(long? stockId, DocumentStatus? status, CancellationToken cancellationToken) =>
            sender.Send(new GetInventoryReceiptListQuery(stockId, status), cancellationToken);

        [HttpPost("{id}/Lines")]
        public Task<Result> AddLine(long id, [FromBody] InventoryReceiptLineInput line, CancellationToken cancellationToken) =>
            sender.Send(new AddInventoryReceiptLineCommand(id, line.ProductId, line.UnitId, line.Quantity, line.UnitCost, line.BatchId, line.Notes), cancellationToken);

        [HttpPut("{id}/Confirm")]
        public Task<Result> Confirm(long id, CancellationToken cancellationToken) =>
            sender.Send(new ConfirmInventoryReceiptCommand(id), cancellationToken);

        [HttpPut("{id}/Post")]
        public Task<Result> Post(long id, CancellationToken cancellationToken) =>
            sender.Send(new PostInventoryReceiptCommand(id), cancellationToken);

        [HttpPut("{id}/Cancel")]
        public Task<Result> Cancel(long id, CancellationToken cancellationToken) =>
            sender.Send(new CancelInventoryReceiptCommand(id), cancellationToken);
    }
}
