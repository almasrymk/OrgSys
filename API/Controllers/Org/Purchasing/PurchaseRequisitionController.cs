using Purchasing.Application;
using Purchasing.Application.PurchaseRequisitions.Commands;
using Purchasing.Application.PurchaseRequisitions.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Purchasing
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseRequisitionController(ISender sender) : BaseController<GetByIdPurchaseRequisitionQuery,
        SearchPurchaseRequisitionQuery, GetListPurchaseRequisitionQuery, CreatePurchaseRequisitionCommand,
        UpdatePurchaseRequisitionCommand, DeletePurchaseRequisitionCommand, DeleteListPurchaseRequisitionCommand,
        GetMaxPurchaseRequisitionQuery, PurchaseRequisitionDto>(sender)
    {
        [HttpPut("Submit")]
        public async Task<Result> Submit(long Id, CancellationToken cancellationToken) =>
            await Sender.Send(new SubmitPurchaseRequisitionCommand(Id), cancellationToken);

        [HttpPut("Reject")]
        public async Task<Result> Reject(long Id, CancellationToken cancellationToken) =>
            await Sender.Send(new RejectPurchaseRequisitionCommand(Id), cancellationToken);

        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken) =>
            await Sender.Send(new CancelPurchaseRequisitionCommand(Id), cancellationToken);

        [HttpPost("ConvertToPurchaseOrder")]
        public async Task<Result<long>> ConvertToPurchaseOrder(long Id, long DealerId, CancellationToken cancellationToken) =>
            await Sender.Send(new ConvertToPurchaseOrderCommand(Id, DealerId), cancellationToken);
    }
}
