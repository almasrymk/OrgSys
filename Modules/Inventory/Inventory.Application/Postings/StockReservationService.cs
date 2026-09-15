namespace Inventory.Application.Postings;

using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// The concurrency-safe reserve operation (brief §15), extracted so both the internal
/// ReserveStockCommandHandler and the cross-module ReserveInventoryCommand contract handler share
/// exactly one retry-on-conflict implementation instead of drifting apart.
/// </summary>
public sealed class StockReservationService(
    IInventoryBalanceRepository balanceRepository,
    IRepository<Stock> stockRepository,
    IRepository<StockReservation> reservationRepository,
    IUnitOfWork unitOfWork)
{
    private const int MaxAttempts = 5;

    public async Task<(bool Success, long ReservationId, string? Error)> ReserveAsync(
        long productId, long stockId, long? locationId, long? batchId, long? serialId, decimal quantity,
        SourceDocumentType sourceType, long sourceId, long? sourceLineId, long createUserId, DateTime? expiresAt,
        CancellationToken cancellationToken)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                var stock = await stockRepository.GetByFilterAsync(s => s.Id == stockId, "");
                stock?.EnsureActiveForPosting();

                var balance = await balanceRepository.GetOrCreateTrackedAsync(productId, stockId, locationId, batchId, cancellationToken);
                balance.Reserve(quantity, stock?.AllowNegativeStock ?? false);

                var reservation = StockReservation.Create(
                    productId, stockId, locationId, batchId, serialId, quantity,
                    sourceType, sourceId, sourceLineId, createUserId, DateTime.Now, expiresAt);

                await reservationRepository.CreateAsync(reservation);
                await unitOfWork.SaveChangeAsync(cancellationToken);

                return (true, reservation.Id, null);
            }
            catch (DbUpdateConcurrencyException) when (attempt < MaxAttempts)
            {
                unitOfWork.ResetDbContextState();
            }
            catch (InventoryDomainException ex)
            {
                return (false, 0, ex.Message);
            }
        }

        return (false, 0, "Could not reserve stock due to a concurrent update; please retry.");
    }
}
