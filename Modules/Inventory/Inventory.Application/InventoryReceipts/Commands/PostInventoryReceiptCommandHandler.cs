namespace Inventory.Application.InventoryReceipts.Commands;

using Inventory.Application.Postings;
using Inventory.Contracts.Receipts;
using OrgSys.SharedKernel;
using System.Net;

public sealed record PostInventoryReceiptCommand(long InventoryReceiptId) : ICommand;

public sealed class PostInventoryReceiptCommandHandler(
    IRepository<InventoryReceipt> receiptRepository,
    InventoryLedgerPoster poster,
    IUnitOfWork unitOfWork,
    IIntegrationEventPublisher integrationEventPublisher) : ICommandHandler<PostInventoryReceiptCommand>
{
    public async Task<Result> Handle(PostInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.GetByFilterAsync(r => r.Id == request.InventoryReceiptId, "Lines");
        if (receipt is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory receipt not found.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            receipt.Post(DateTime.Now);
            await poster.PostReceiptAsync(receipt, cancellationToken);

            await receiptRepository.UpdateAsync(receipt);
            await integrationEventPublisher.PublishAsync(
                new GoodsReceiptPostedIntegrationEvent(
                    receipt.Id,
                    receipt.StockId,
                    receipt.SourceId,
                    DateTime.UtcNow,
                    receipt.PurchaseOrderId,
                    receipt.Lines.Select(l => new GoodsReceiptPostedLine(l.ProductId, l.UnitId, l.Quantity)).ToList()),
                cancellationToken);
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
