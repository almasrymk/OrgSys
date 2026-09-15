using Inventory.Application.InventoryBalances.Queries;
using Inventory.Application.StockCard.Queries;
using Inventory.Contracts.Availability;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrgSys.SharedKernel;

namespace API.Controllers.Org.Inventory
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InventoryBalancesController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public Task<Result<StockBalanceDto>> Get(long productId, long stockId, long? locationId, long? batchId, CancellationToken cancellationToken) =>
            sender.Send(new GetStockBalanceQuery(productId, stockId, locationId, batchId), cancellationToken);

        [HttpGet("List")]
        public Task<ResultCollection<StockBalanceDto>> GetList(long? stockId, long? productId, CancellationToken cancellationToken) =>
            sender.Send(new GetStockBalancesQuery(stockId, productId), cancellationToken);

        [HttpGet("StockCard")]
        public Task<ResultCollection<StockCardLineDto>> GetStockCard(long productId, long? stockId, DateTime? dateFrom, DateTime? dateTo, CancellationToken cancellationToken) =>
            sender.Send(new GetStockCardQuery(productId, stockId, dateFrom, dateTo), cancellationToken);

        [HttpGet("Availability")]
        public Task<Result<InventoryAvailabilityDto>> GetAvailability(long productId, long stockId, long? locationId, long? batchId, decimal requestedQuantity, CancellationToken cancellationToken) =>
            sender.Send(new GetInventoryAvailabilityQuery(productId, stockId, locationId, batchId, requestedQuantity), cancellationToken);
    }
}
