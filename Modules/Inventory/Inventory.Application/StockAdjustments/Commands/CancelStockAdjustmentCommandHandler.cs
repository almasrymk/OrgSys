namespace Inventory.Application.StockAdjustments.Commands;

using System.Net;

public sealed record CancelStockAdjustmentCommand(long StockAdjustmentId) : ICommand;

public sealed class CancelStockAdjustmentCommandHandler(
    IRepository<StockAdjustment> adjustmentRepository, IUnitOfWork unitOfWork) : ICommandHandler<CancelStockAdjustmentCommand>
{
    public async Task<Result> Handle(CancelStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        var adjustment = await adjustmentRepository.GetByFilterAsync(a => a.Id == request.StockAdjustmentId, "");
        if (adjustment is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Stock adjustment not found.")]);

        try
        {
            adjustment.Cancel();
            await adjustmentRepository.UpdateAsync(adjustment);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}
