namespace Inventory.Application.TransactionTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateTransactionTypeCommand : TransactionTypeDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.TransactionType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTransactionTypeCommand, Inventory.Domain.TransactionType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}