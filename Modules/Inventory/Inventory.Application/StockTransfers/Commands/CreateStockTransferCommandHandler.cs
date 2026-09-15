namespace Inventory.Application.StockTransfers.Commands;

using System.Net;

public sealed record StockTransferLineInput(long ProductId, long UnitId, decimal Quantity, long? BatchId, string? Notes);

public sealed record CreateStockTransferCommand(
    long FromStockId, long ToStockId, DateTime Date, long CreateUserId, long? BranchId,
    string? Notes, List<StockTransferLineInput> Lines) : ICommand<long>;

public sealed class CreateStockTransferCommandHandler(
    IRepository<StockTransfer> transferRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateStockTransferCommand, long>
{
    public async Task<Result<long>> Handle(CreateStockTransferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var transfer = StockTransfer.Create(request.FromStockId, request.ToStockId, request.Date, request.CreateUserId, DateTime.Now, request.BranchId, request.Notes);

            foreach (var line in request.Lines)
                transfer.AddLine(line.ProductId, line.UnitId, line.Quantity, line.BatchId, line.Notes);

            await transferRepository.CreateAsync(transfer);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            return new Result<long>(HttpStatusCode.OK, transfer.Id, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
