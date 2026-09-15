namespace Inventory.Application.StockTransfers.Commands;

using System.Net;

public sealed record AddStockTransferLineCommand(long StockTransferId, long ProductId, long UnitId, decimal Quantity, long? BatchId, string? Notes) : ICommand;

public sealed class AddStockTransferLineCommandHandler(
    IRepository<StockTransfer> transferRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddStockTransferLineCommand>
{
    public async Task<Result> Handle(AddStockTransferLineCommand request, CancellationToken cancellationToken)
    {
        var transfer = await transferRepository.GetByFilterAsync(t => t.Id == request.StockTransferId, "Lines");
        if (transfer is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Stock transfer not found.")]);

        try
        {
            transfer.AddLine(request.ProductId, request.UnitId, request.Quantity, request.BatchId, request.Notes);
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
