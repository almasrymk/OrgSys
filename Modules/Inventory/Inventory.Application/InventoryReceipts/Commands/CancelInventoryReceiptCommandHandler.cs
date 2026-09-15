namespace Inventory.Application.InventoryReceipts.Commands;

using System.Net;

public sealed record CancelInventoryReceiptCommand(long InventoryReceiptId) : ICommand;

public sealed class CancelInventoryReceiptCommandHandler(
    IRepository<InventoryReceipt> receiptRepository, IUnitOfWork unitOfWork) : ICommandHandler<CancelInventoryReceiptCommand>
{
    public async Task<Result> Handle(CancelInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.GetByFilterAsync(r => r.Id == request.InventoryReceiptId, "");
        if (receipt is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory receipt not found.")]);

        try
        {
            receipt.Cancel();
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
