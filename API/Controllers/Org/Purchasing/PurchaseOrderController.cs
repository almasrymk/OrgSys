using Purchasing.Application;
using Purchasing.Application.PurchaseOrders.Commands;
using Purchasing.Application.PurchaseOrders.Queries;
using Purchasing.Contracts.PurchaseOrders;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Purchasing
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseOrderController(ISender sender) : BaseController<GetByIdPurchaseOrderQuery,
        SearchPurchaseOrderQuery, GetListPurchaseOrderQuery, CreatePurchaseOrderCommand,
        UpdatePurchaseOrderCommand, DeletePurchaseOrderCommand, DeleteListPurchaseOrderCommand,
        GetMaxPurchaseOrderQuery, PurchaseOrderDto>(sender)
    {
        [HttpPut("LinkInvoice")]
        public async Task<Result> LinkInvoice(long Id, long InvoiceId, CancellationToken cancellationToken) =>
            await Sender.Send(new LinkInvoiceCommand(Id, InvoiceId), cancellationToken);

        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken) =>
            await Sender.Send(new CancelPurchaseOrderCommand(Id), cancellationToken);

        [HttpGet("Remaining")]
        public Task<Result<PurchaseOrderRemainingDto?>> Remaining(long purchaseOrderId, CancellationToken cancellationToken) =>
            Sender.Send(new GetPurchaseOrderRemainingQuery(purchaseOrderId), cancellationToken);

        [HttpGet("ThreeWayMatch")]
        public Task<Result<ThreeWayMatchDto?>> ThreeWayMatch(long purchaseOrderId, CancellationToken cancellationToken) =>
            Sender.Send(new GetThreeWayMatchQuery(purchaseOrderId), cancellationToken);
    }
}
