namespace Inventory.Application.InventoryBatches.Commands;

using System.Net;

public sealed record QuarantineBatchCommand(long BatchId) : ICommand;
public sealed record ActivateBatchCommand(long BatchId) : ICommand;

public sealed class QuarantineBatchCommandHandler(
    IRepository<InventoryBatch> batchRepository, IUnitOfWork unitOfWork) : ICommandHandler<QuarantineBatchCommand>
{
    public async Task<Result> Handle(QuarantineBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await batchRepository.GetByFilterAsync(b => b.Id == request.BatchId, "");
        if (batch is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Batch not found.")]);

        batch.Quarantine();
        await batchRepository.UpdateAsync(batch);
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return new Result(HttpStatusCode.OK, null);
    }
}

public sealed class ActivateBatchCommandHandler(
    IRepository<InventoryBatch> batchRepository, IUnitOfWork unitOfWork) : ICommandHandler<ActivateBatchCommand>
{
    public async Task<Result> Handle(ActivateBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await batchRepository.GetByFilterAsync(b => b.Id == request.BatchId, "");
        if (batch is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Batch not found.")]);

        batch.Activate();
        await batchRepository.UpdateAsync(batch);
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return new Result(HttpStatusCode.OK, null);
    }
}
