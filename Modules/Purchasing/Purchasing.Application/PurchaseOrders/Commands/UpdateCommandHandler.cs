namespace Purchasing.Application.PurchaseOrders.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdatePurchaseOrderCommand : PurchaseOrderDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<PurchaseOrder> _Repository,
        IRepository<PurchaseOrderProduct> _PurchaseOrderProductRepository,
        IMapper mapper, IServiceProvider _provider)
        : UpdateCommandHandler<UpdatePurchaseOrderCommand, PurchaseOrder>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<bool> SaveDetials(UpdatePurchaseOrderCommand request)
        {
            var ids = request.PurchaseOrderProductList!.Select(e => e.Id);
            var removeList = await _PurchaseOrderProductRepository.GetListByFilterAsync(e => e.PurchaseOrderId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<PurchaseOrderProduct>(removeList!);
            if (!res) return false;

            var lines = mapper.Map<List<PurchaseOrderProduct>>(request.PurchaseOrderProductList);
            res = await UpdateDetails<PurchaseOrderProduct>(lines);
            if (!res) return false;

            return await _Repository.UpdateAsync(request);
        }
    }
}
