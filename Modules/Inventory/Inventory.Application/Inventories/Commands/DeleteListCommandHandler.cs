namespace Inventory.Application.Inventories.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;
    using Inventory.Application.Inventories.Integration;

    public sealed record DeleteListInventoryCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Inventory.Domain.Inventory> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListInventoryCommand, Inventory.Domain.Inventory>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.Inventory, bool>> CreateFilter(DeleteListInventoryCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListInventoryCommand request)
        {

            var inventories = await _Repository.GetListByFilterAsync(t => request.Ids.Contains(t.Id), "InventoryProducts");

            if (inventories == null)
                return false;

            var integration = new InventoryAdjustmentIntegration(_provider);
            foreach (var inventory in inventories)
            {
                await integration.DeleteAsync(inventory.Id);
                inventory.InventoryProducts?.Clear();
            }
            
            return true;
        }
    }
}
