namespace Application.Commands.Org.Transactions.Inventory.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Invoices.Invoice.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteInventoryCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Entity.Model.Inventory> _Repository,
        IRepository<InventoryProduct> _TransactionProductRepository,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteInventoryCommand, Entity.Model.Inventory>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Entity.Model.Inventory, bool>> CreateFilter(DeleteInventoryCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteInventoryCommand request)
        {

          var inventoryProducts =  await _Repository.GetByFilterAsync(t => t.Id == request.Id, "InventoryProducts");

            if(inventoryProducts != null && inventoryProducts.InventoryProducts.Count > 0)
            {
                inventoryProducts.InventoryProducts.Clear();
                return true;
            }

            return false;
        }
    }
}