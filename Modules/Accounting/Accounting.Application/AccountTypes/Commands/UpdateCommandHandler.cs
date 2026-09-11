namespace Accounting.Application.AccountTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateAccountTypeCommand(long Id , long? AccountTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.AccountType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateAccountTypeCommand, Accounting.Domain.AccountType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}