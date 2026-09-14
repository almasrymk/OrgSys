namespace Accounting.Application.Accounts.Commands
{
    using OrgSys.SharedKernel;   
    using AutoMapper;

    public sealed class CreateAccountCommand : Accounting.Application.AccountDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.Account> _Repository , IMapper mapper) : CreateCommandHandler<CreateAccountCommand, Accounting.Domain.Account>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}