namespace Inventory.Application.StockAdjustments.Commands;

using Inventory.Application.Postings;
using System.Net;

public sealed record PostStockAdjustmentCommand(long StockAdjustmentId) : ICommand;

public sealed class PostStockAdjustmentCommandHandler(
    IRepository<StockAdjustment> adjustmentRepository,
    InventoryLedgerPoster poster,
    IUnitOfWork unitOfWork) : ICommandHandler<PostStockAdjustmentCommand>
{
    public async Task<Result> Handle(PostStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        var adjustment = await adjustmentRepository.GetByFilterAsync(a => a.Id == request.StockAdjustmentId, "Lines");
        if (adjustment is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Stock adjustment not found.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            adjustment.Post(DateTime.Now);
            await poster.PostAdjustmentAsync(adjustment, cancellationToken);

            await adjustmentRepository.UpdateAsync(adjustment);
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
