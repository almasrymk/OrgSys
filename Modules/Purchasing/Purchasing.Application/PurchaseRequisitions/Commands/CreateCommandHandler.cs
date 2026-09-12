namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreatePurchaseRequisitionCommand : PurchaseRequisitionDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<PurchaseRequisition> _Repository, IMapper mapper)
        : CreateCommandHandler<CreatePurchaseRequisitionCommand, PurchaseRequisition>(_UnitOfWork, _Repository, mapper)
    {
    }
}
