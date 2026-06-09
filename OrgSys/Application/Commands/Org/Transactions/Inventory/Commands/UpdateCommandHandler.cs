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

    public sealed class UpdateInventoryCommand : Entity.ModelView.InventoryModelView, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Entity.Model.Inventory> _Repository , 
        IRepository<InventoryProduct> _InventoryProductRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInventoryCommand, Entity.Model.Inventory>(_UnitOfWork, _Repository , mapper , _provider)
    {
        override public async Task<bool> SaveDetials(UpdateInventoryCommand request)
        {
            #region UpdateProduct
            var ids = request.InventoryProductList.Select(e => e.Id);
            var removeList = await _InventoryProductRepository.GetListByFilterAsync(e => e.InventoryId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<InventoryProduct>(removeList!);
            if (!res) return false;
            var ob = mapper.Map<List<InventoryProduct>>(request.InventoryProductList);
            res = await UpdateDetails<InventoryProduct>(ob);
            #endregion 

            return res;
        }
    }
}