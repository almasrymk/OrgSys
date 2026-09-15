namespace Inventory.Application.StockReservations.Commands;

using Inventory.Domain.Repositories;
using System.Net;

/// <summary>Fulfillment (e.g. a Sales Delivery consuming its reservation) physically removes the
/// reserved stock — this is the ONLY path allowed to draw OnHand down for previously-reserved
/// quantity (brief §14); it does not itself create an InventoryIssue/InventoryMovement — the
/// delivery flow calls this alongside posting its own InventoryIssue for the same quantity, exactly
/// as InventoryBalance.IssueOut vs FulfillReservation are kept distinct in the domain layer.</summary>
public sealed record FulfillStockReservationCommand(long StockReservationId) : ICommand;

public sealed class FulfillStockReservationCommandHandler(
    IRepository<StockReservation> reservationRepository,
    IInventoryBalanceRepository balanceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<FulfillStockReservationCommand>
{
    public async Task<Result> Handle(FulfillStockReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetByFilterAsync(r => r.Id == request.StockReservationId, "");
        if (reservation is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Reservation not found.")]);

        try
        {
            var balance = await balanceRepository.GetTrackedAsync(reservation.ProductId, reservation.StockId, reservation.LocationId, reservation.BatchId, cancellationToken);
            if (balance is null)
                return new Result(HttpStatusCode.BadRequest, [new Error("No balance exists for this reservation's item/warehouse.")]);

            balance.FulfillReservation(reservation.Quantity);
            reservation.Fulfill(DateTime.Now);

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
