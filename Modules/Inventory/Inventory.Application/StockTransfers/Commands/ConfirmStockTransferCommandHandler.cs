namespace Inventory.Application.StockTransfers.Commands;

using System.Net;

public sealed record ConfirmStockTransferCommand(long StockTransferId) : ICommand;

public sealed class ConfirmStockTransferCommandHandler(
    IRepository<StockTransfer> transferRepository, IUnitOfWork unitOfWork) : ICommandHandler<ConfirmStockTransferCommand>
{
    public async Task<Result> Handle(ConfirmStockTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = await transferRepository.GetByFilterAsync(t => t.Id == request.StockTransferId, "Lines");
        if (transfer is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Stock transfer not found.")]);

        try
        {
            transfer.Confirm();
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
