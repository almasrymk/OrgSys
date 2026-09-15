namespace Inventory.Application.InventoryBatches.Commands;

using System.Net;

/// <summary>Creates (or, if the batch number already exists for this item, returns the existing)
/// InventoryBatch — the id this returns is what a caller passes as BatchId when adding a line to an
/// InventoryReceipt/Issue/Transfer/Adjustment (brief §16/§37 "AssignBatchCommand").</summary>
public sealed record AssignBatchCommand(
    long ProductId, string BatchNumber, DateTime? ManufacturingDate, DateTime? ExpiryDate, string? SupplierBatchNumber) : ICommand<long>;

public sealed class AssignBatchCommandHandler(
    IRepository<InventoryBatch> batchRepository, IUnitOfWork unitOfWork) : ICommandHandler<AssignBatchCommand, long>
{
    public async Task<Result<long>> Handle(AssignBatchCommand request, CancellationToken cancellationToken)
    {
        var existing = await batchRepository.GetByFilterAsync(
            b => b.ProductId == request.ProductId && b.BatchNumber == request.BatchNumber, "");
        if (existing is not null)
            return new Result<long>(HttpStatusCode.OK, existing.Id, null);

        try
        {
            var batch = InventoryBatch.Create(request.ProductId, request.BatchNumber, request.ManufacturingDate, request.ExpiryDate, request.SupplierBatchNumber);
            await batchRepository.CreateAsync(batch);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result<long>(HttpStatusCode.OK, batch.Id, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
