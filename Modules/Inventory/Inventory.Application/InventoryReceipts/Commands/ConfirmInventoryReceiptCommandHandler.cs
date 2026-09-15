namespace Inventory.Application.InventoryReceipts.Commands;

using System.Net;

public sealed record ConfirmInventoryReceiptCommand(long InventoryReceiptId) : ICommand;

public sealed class ConfirmInventoryReceiptCommandHandler(
    IRepository<InventoryReceipt> receiptRepository, IUnitOfWork unitOfWork) : ICommandHandler<ConfirmInventoryReceiptCommand>
{
    public async Task<Result> Handle(ConfirmInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.GetByFilterAsync(r => r.Id == request.InventoryReceiptId, "Lines");
        if (receipt is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory receipt not found.")]);

        try
        {
            receipt.Confirm();
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
