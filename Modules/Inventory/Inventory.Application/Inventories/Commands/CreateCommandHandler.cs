namespace Inventory.Application.Inventories.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Inventory.Application.Inventories.Integration;
    using System.Net;

    public sealed class CreateInventoryCommand : InventoryDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Inventory> _Repository,
        IMapper mapper, IServiceProvider provider) : CreateCommandHandler<CreateInventoryCommand, Inventory.Domain.Inventory>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var inventory = mapper.Map<Inventory.Domain.Inventory>(request);
                await _Repository.CreateAsync(inventory);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                await new InventoryAdjustmentIntegration(provider).SyncAsync(inventory, cancellationToken);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);
                await _UnitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
