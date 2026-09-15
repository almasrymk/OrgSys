namespace Inventory.Application.InventoryReceipts.Commands;

using System.Net;

public sealed record AddInventoryReceiptLineCommand(
    long InventoryReceiptId, long ProductId, long UnitId, decimal Quantity, decimal UnitCost, long? BatchId, string? Notes) : ICommand;

public sealed class AddInventoryReceiptLineCommandHandler(
    IRepository<InventoryReceipt> receiptRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddInventoryReceiptLineCommand>
{
    public async Task<Result> Handle(AddInventoryReceiptLineCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.GetByFilterAsync(r => r.Id == request.InventoryReceiptId, "Lines");
        if (receipt is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory receipt not found.")]);

        try
        {
            receipt.AddLine(request.ProductId, request.UnitId, request.Quantity, request.UnitCost, request.BatchId, request.Notes);
            await receiptRepository.UpdateAsync(receipt);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}
