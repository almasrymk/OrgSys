namespace Application.Commands.Org.Transactions.Inventory.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Invoices.Invoice.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;

    public sealed class UpdateInventoryCommand : Application.DTOs.InventoryDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Domain.Entities.Inventory> _Repository , 
        IRepository<InventoryProduct> _InventoryProductRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInventoryCommand, Domain.Entities.Inventory>(_UnitOfWork, _Repository , mapper , _provider)
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