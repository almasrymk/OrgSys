using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using System.Net;
using Inventory.Application.Inventories.Integration;

namespace Inventory.Application.Inventories.Commands
{
    public sealed record CancelInventoryCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public sealed class CancelInventoryCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<Inventory.Domain.Inventory> repository,
        IMapper mapper,
        IServiceProvider provider)
        : UpdateCommandHandler<CancelInventoryCommand, Inventory.Domain.Inventory>(unitOfWork, repository, mapper, provider)
    {
        public override async Task<Result> Handle(CancelInventoryCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var inventory = await repository.GetByFilterAsync(x => x.Id == request.Id, string.Empty);
                if (inventory == null)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Inventory not found") });
                }

                if (inventory.Status == OrgSys.SharedKernel.Status.Cancel)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.OK, null);
                }

                inventory.Status = OrgSys.SharedKernel.Status.Cancel;
                await new InventoryAdjustmentIntegration(provider)
                    .SetStatusAsync(inventory.Id, OrgSys.SharedKernel.Status.Cancel);
                var saved = await unitOfWork.SaveChangeAsync(cancellationToken);
                if (saved > 0)
                {
                    await unitOfWork.CommitAsync();
                    return new Result(HttpStatusCode.OK, null);
                }

                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error saving changes") });
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error(ex.Message) });
            }
        }
    }
}
