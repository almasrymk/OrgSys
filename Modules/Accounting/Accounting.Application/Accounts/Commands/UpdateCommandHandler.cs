namespace Accounting.Application.Accounts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateAccountCommand : Accounting.Application.AccountDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.Account> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateAccountCommand, Accounting.Domain.Account>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}