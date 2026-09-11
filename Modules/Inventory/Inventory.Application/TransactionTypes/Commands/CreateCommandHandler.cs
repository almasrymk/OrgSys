namespace Inventory.Application.TransactionTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateTransactionTypeCommand : TransactionTypeDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.TransactionType> _Repository , IMapper mapper) : CreateCommandHandler<CreateTransactionTypeCommand, Inventory.Domain.TransactionType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}