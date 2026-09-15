using Inventory.Application.StockReservations.Commands;
using Inventory.Application.StockReservations.Queries;
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
    public class StockReservationsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public Task<Result<long>> Reserve(ReserveStockCommand command, CancellationToken cancellationToken) =>
            sender.Send(command, cancellationToken);

        [HttpPut("{id}/Release")]
        public Task<Result> Release(long id, CancellationToken cancellationToken) =>
            sender.Send(new ReleaseStockReservationCommand(id), cancellationToken);

        [HttpPut("{id}/Fulfill")]
        public Task<Result> Fulfill(long id, CancellationToken cancellationToken) =>
            sender.Send(new FulfillStockReservationCommand(id), cancellationToken);

        [HttpGet]
        public Task<ResultCollection<ReservationDto>> GetList(long? productId, long? stockId, ReservationStatus? status, CancellationToken cancellationToken) =>
            sender.Send(new GetReservationsQuery(productId, stockId, status), cancellationToken);
    }
}
