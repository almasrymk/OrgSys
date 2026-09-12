namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdatePurchaseRequisitionCommand : PurchaseRequisitionDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<PurchaseRequisition> _Repository,
        IRepository<PurchaseRequisitionProduct> _PurchaseRequisitionProductRepository,
        IMapper mapper, IServiceProvider _provider)
        : UpdateCommandHandler<UpdatePurchaseRequisitionCommand, PurchaseRequisition>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<bool> SaveDetials(UpdatePurchaseRequisitionCommand request)
        {
            var ids = request.PurchaseRequisitionProductList!.Select(e => e.Id);
            var removeList = await _PurchaseRequisitionProductRepository.GetListByFilterAsync(e => e.PurchaseRequisitionId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<PurchaseRequisitionProduct>(removeList!);
            if (!res) return false;

            var lines = mapper.Map<List<PurchaseRequisitionProduct>>(request.PurchaseRequisitionProductList);
            res = await UpdateDetails<PurchaseRequisitionProduct>(lines);
            if (!res) return false;

            return await _Repository.UpdateAsync(request);
        }
    }
}
