namespace Inventory.Application.Inventories.Commands;

using OrgSys.SharedKernel;
using Inventory.Application.Inventories.Integration;
using System.Net;

public sealed record CreateAdjustmentCommand(long InventoryId) : ICommand;

public sealed class CreateAdjustmentCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<global::Inventory.Domain.Inventory> inventoryRepository,
    IServiceProvider provider) : ICommandHandler<CreateAdjustmentCommand>
{
    public async Task<Result> Handle(CreateAdjustmentCommand request, CancellationToken cancellationToken)
    {
        var inventory = await inventoryRepository.GetByFilterAsync(
            e => e.Id == request.InventoryId && e.Status != OrgSys.SharedKernel.Status.Deleted,
            "InventoryProducts");

        if (inventory == null)
            return new Result(HttpStatusCode.NotFound, [new Error("Inventory not found.")]);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            await new InventoryAdjustmentIntegration(provider).SyncAsync(inventory, cancellationToken, true);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            await unitOfWork.CommitAsync();
            return new Result(HttpStatusCode.OK, null);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}
