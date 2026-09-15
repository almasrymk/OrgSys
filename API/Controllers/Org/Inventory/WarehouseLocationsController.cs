using Inventory.Application;
using Inventory.Application.WarehouseLocations.Commands;
using Inventory.Application.WarehouseLocations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WarehouseLocationsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public Task<Result> Create(CreateWarehouseLocationCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpGet("ByStock")]
        public Task<ResultCollection<WarehouseLocationDto>> GetByStock(long stockId, CancellationToken cancellationToken) =>
            sender.Send(new GetWarehouseLocationsByStockQuery(stockId), cancellationToken);
    }
}
