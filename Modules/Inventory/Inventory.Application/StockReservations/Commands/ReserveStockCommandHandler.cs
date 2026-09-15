namespace Inventory.Application.StockReservations.Commands;

using Inventory.Application.Postings;
using System.Net;

public sealed record ReserveStockCommand(
    long ProductId, long StockId, long? LocationId, long? BatchId, long? SerialId, decimal Quantity,
    SourceDocumentType SourceType, long SourceId, long? SourceLineId, long CreateUserId, DateTime? ExpiresAt) : ICommand<long>;

/// <summary>Thin CQRS entry point — the actual concurrency-safe reserve logic (brief §15) lives in
/// StockReservationService, shared with the cross-module ReserveInventoryCommand contract
/// handler.</summary>
public sealed class ReserveStockCommandHandler(StockReservationService reservationService) : ICommandHandler<ReserveStockCommand, long>
{
    public async Task<Result<long>> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var (success, reservationId, error) = await reservationService.ReserveAsync(
            request.ProductId, request.StockId, request.LocationId, request.BatchId, request.SerialId, request.Quantity,
            request.SourceType, request.SourceId, request.SourceLineId, request.CreateUserId, request.ExpiresAt, cancellationToken);

        return success
            ? new Result<long>(HttpStatusCode.OK, reservationId, null)
            : new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(error!)]);
    }
}
