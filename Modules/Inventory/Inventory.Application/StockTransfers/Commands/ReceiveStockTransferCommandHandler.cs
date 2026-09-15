namespace Inventory.Application.StockTransfers.Commands;

using Inventory.Application.Postings;
using System.Net;

/// <summary>Posts the receipt side at ToStockId for a Shipped (in-transit) transfer that was NOT
/// already received immediately by ShipStockTransferCommand(Immediate: true).</summary>
public sealed record ReceiveStockTransferCommand(long StockTransferId) : ICommand;

public sealed class ReceiveStockTransferCommandHandler(
    IRepository<StockTransfer> transferRepository,
    InventoryLedgerPoster poster,
    IUnitOfWork unitOfWork) : ICommandHandler<ReceiveStockTransferCommand>
{
    public async Task<Result> Handle(ReceiveStockTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = await transferRepository.GetByFilterAsync(t => t.Id == request.StockTransferId, "Lines");
        if (transfer is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Stock transfer not found.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            transfer.Receive(DateTime.Now);
            await poster.PostTransferReceiptAsync(transfer, cancellationToken);

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
