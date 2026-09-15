namespace Inventory.Application.StockTransfers.Commands;

using System.Net;

public sealed record CancelStockTransferCommand(long StockTransferId) : ICommand;

public sealed class CancelStockTransferCommandHandler(
    IRepository<StockTransfer> transferRepository, IUnitOfWork unitOfWork) : ICommandHandler<CancelStockTransferCommand>
{
    public async Task<Result> Handle(CancelStockTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = await transferRepository.GetByFilterAsync(t => t.Id == request.StockTransferId, "");
        if (transfer is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Stock transfer not found.")]);

        try
        {
            transfer.Cancel();
            await transferRepository.UpdateAsync(transfer);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}
