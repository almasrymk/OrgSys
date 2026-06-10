namespace Application.Commands.Org.Transactions.Inventory.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListInventoryCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Entity.Model.Inventory> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListInventoryCommand, Entity.Model.Inventory>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Inventory, bool>> CreateFilter(DeleteListInventoryCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListInventoryCommand request)
        {

            var inventories = await _Repository.GetListByFilterAsync(t => request.Ids.Contains(t.Id), "InventoryProducts");

            if (inventories == null || !inventories.Any())
                return false;

            foreach (var inventoryProduct in inventories)
                inventoryProduct.InventoryProducts.Clear();
            
            return true;
        }
    }
}