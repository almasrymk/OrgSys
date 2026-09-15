namespace Inventory.Application.StockAdjustments.Commands;

using System.Net;

public sealed record StockAdjustmentLineInput(long ProductId, long UnitId, MovementDirection Direction, decimal Quantity, long? BatchId, string? Notes);

public sealed record CreateStockAdjustmentCommand(
    long StockId, long ReasonId, DateTime Date, long CreateUserId, long? BranchId,
    string? Notes, List<StockAdjustmentLineInput> Lines) : ICommand<long>;

public sealed class CreateStockAdjustmentCommandHandler(
    IRepository<StockAdjustment> adjustmentRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateStockAdjustmentCommand, long>
{
    public async Task<Result<long>> Handle(CreateStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var adjustment = StockAdjustment.Create(request.StockId, request.ReasonId, request.Date, request.CreateUserId, DateTime.Now, request.BranchId, request.Notes);

            foreach (var line in request.Lines)
                adjustment.AddLine(line.ProductId, line.UnitId, line.Direction, line.Quantity, line.BatchId, line.Notes);

            await adjustmentRepository.CreateAsync(adjustment);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            return new Result<long>(HttpStatusCode.OK, adjustment.Id, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
