namespace Accounting.Application.AccountTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateAccountTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.AccountType> _Repository , IMapper mapper) : CreateCommandHandler<CreateAccountTypeCommand, Accounting.Domain.AccountType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}