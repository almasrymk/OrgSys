namespace Application.Commands.Org.Transactions.Inventory.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Application.Commands.Org.Transactions.Inventory.Integration;

    public sealed record DeleteListInventoryCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Inventory> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListInventoryCommand, Domain.Entities.Inventory>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Inventory, bool>> CreateFilter(DeleteListInventoryCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
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
