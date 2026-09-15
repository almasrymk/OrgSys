using Inventory.Application;
using Inventory.Application.StockAdjustmentReasons.Commands;
using Inventory.Application.StockAdjustmentReasons.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StockAdjustmentReasonsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public Task<Result> Create(CreateStockAdjustmentReasonCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpGet]
        public Task<ResultCollection<StockAdjustmentReasonDto>> GetList(CancellationToken cancellationToken) =>
            sender.Send(new GetStockAdjustmentReasonListQuery(), cancellationToken);
    }
}
