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

            if (inventories == null || !inventories.Any())
                return false;

            foreach (var inventoryProduct in inventories)
                inventoryProduct.InventoryProducts.Clear();
            
            return true;
        }
    }
}