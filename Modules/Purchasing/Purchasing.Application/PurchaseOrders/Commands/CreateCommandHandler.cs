namespace Purchasing.Application.PurchaseOrders.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreatePurchaseOrderCommand : PurchaseOrderDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<PurchaseOrder> _Repository, IMapper mapper)
        : CreateCommandHandler<CreatePurchaseOrderCommand, PurchaseOrder>(_UnitOfWork, _Repository, mapper)
    {
    }
}
