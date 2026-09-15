namespace Inventory.Application.StockReservations.Commands;

using Inventory.Domain.Repositories;
using System.Net;

public sealed record ReleaseStockReservationCommand(long StockReservationId) : ICommand;

public sealed class ReleaseStockReservationCommandHandler(
    IRepository<StockReservation> reservationRepository,
    IInventoryBalanceRepository balanceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ReleaseStockReservationCommand>
{
    public async Task<Result> Handle(ReleaseStockReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetByFilterAsync(r => r.Id == request.StockReservationId, "");
        if (reservation is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Reservation not found.")]);

        try
        {
            var balance = await balanceRepository.GetTrackedAsync(reservation.ProductId, reservation.StockId, reservation.LocationId, reservation.BatchId, cancellationToken);
            if (balance is null)
                return new Result(HttpStatusCode.BadRequest, [new Error("No balance exists for this reservation's item/warehouse.")]);

            balance.ReleaseReservation(reservation.Quantity);
            reservation.Release(DateTime.Now);

            await reservationRepository.UpdateAsync(reservation);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}
