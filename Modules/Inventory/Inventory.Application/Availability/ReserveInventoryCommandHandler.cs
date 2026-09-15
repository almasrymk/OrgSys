namespace Inventory.Application.Availability;

using Inventory.Application.Postings;
using Inventory.Contracts.Availability;
using System.Net;

public sealed class ReserveInventoryCommandHandler(StockReservationService reservationService) : ICommandHandler<ReserveInventoryCommand, long>
{
    public async Task<Result<long>> Handle(ReserveInventoryCommand request, CancellationToken cancellationToken)
    {
        var sourceType = request.SourceType switch
        {
            ReservationSourceType.SalesOrder => SourceDocumentType.SalesOrder,
            _ => SourceDocumentType.SalesOrder
        };

        var (success, reservationId, error) = await reservationService.ReserveAsync(
            request.ProductId, request.StockId, request.LocationId, request.BatchId, serialId: null, request.Quantity,
            sourceType, request.SourceId, request.SourceLineId, request.CreateUserId, expiresAt: null, cancellationToken);

        return success
            ? new Result<long>(HttpStatusCode.OK, reservationId, null)
            : new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(error!)]);
    }
}
