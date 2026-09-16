namespace Inventory.Application.InventoryReceipts.Commands;

using System.Net;

public sealed record InventoryReceiptLineInput(long ProductId, long UnitId, decimal Quantity, decimal UnitCost, long? BatchId, string? Notes);

public sealed record CreateInventoryReceiptCommand(
    long StockId, long? LocationId, long? DealerId, DateTime Date, long CreateUserId, long? BranchId,
    string? Notes, List<InventoryReceiptLineInput> Lines, long? PurchaseOrderId = null) : ICommand<long>;

public sealed class CreateInventoryReceiptCommandHandler(
    IRepository<InventoryReceipt> receiptRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateInventoryReceiptCommand, long>
{
    public async Task<Result<long>> Handle(CreateInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var receipt = InventoryReceipt.Create(
                request.StockId, request.LocationId, request.DealerId, request.Date,
                request.CreateUserId, DateTime.Now, request.BranchId, request.Notes,
                purchaseOrderId: request.PurchaseOrderId);

            foreach (var line in request.Lines)
                receipt.AddLine(line.ProductId, line.UnitId, line.Quantity, line.UnitCost, line.BatchId, line.Notes);

            await receiptRepository.CreateAsync(receipt);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            return new Result<long>(HttpStatusCode.OK, receipt.Id, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
