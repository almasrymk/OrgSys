namespace Inventory.Application.StockTransfers.Commands;

using Inventory.Application.Postings;
using System.Net;

/// <summary>Posts the issue side at FromStockId. When <paramref name="Immediate"/> is true, also
/// posts the receipt side at ToStockId in the same call — matching today's "AutoReceived"
/// Preference-driven behavior in TransferReceivedIntegration for warehouses/items that don't need a
/// transit state (brief §11: the transit workflow is optional, not forced).</summary>
public sealed record ShipStockTransferCommand(long StockTransferId, bool Immediate = true) : ICommand;

public sealed class ShipStockTransferCommandHandler(
    IRepository<StockTransfer> transferRepository,
    InventoryLedgerPoster poster,
    IUnitOfWork unitOfWork) : ICommandHandler<ShipStockTransferCommand>
{
    public async Task<Result> Handle(ShipStockTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = await transferRepository.GetByFilterAsync(t => t.Id == request.StockTransferId, "Lines");
        if (transfer is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Stock transfer not found.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            transfer.Ship(DateTime.Now);
            await poster.PostTransferShipmentAsync(transfer, cancellationToken);

            if (request.Immediate)
            {
                transfer.Receive(DateTime.Now);
                await poster.PostTransferReceiptAsync(transfer, cancellationToken);
            }

            await transferRepository.UpdateAsync(transfer);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            await unitOfWork.CommitAsync();
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}
