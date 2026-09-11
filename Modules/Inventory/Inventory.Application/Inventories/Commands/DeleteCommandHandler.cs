namespace Inventory.Application.Inventories.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Inventory.Application.Inventories.Integration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteInventoryCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Inventory.Domain.Inventory> _Repository,
        IRepository<InventoryProduct> _TransactionProductRepository,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteInventoryCommand, Inventory.Domain.Inventory>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Inventory.Domain.Inventory, bool>> CreateFilter(DeleteInventoryCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteInventoryCommand request)
        {

          var inventoryProducts =  await _Repository.GetByFilterAsync(t => t.Id == request.Id, "InventoryProducts");

            if (inventoryProducts == null)
                return false;

            await new InventoryAdjustmentIntegration(_provider).DeleteAsync(request.Id);
            inventoryProducts.InventoryProducts?.Clear();
            return true;
        }
    }
}
