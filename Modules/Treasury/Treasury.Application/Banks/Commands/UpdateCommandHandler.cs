namespace Treasury.Application.Banks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateBankCommand : BankDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Bank> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateBankCommand, Treasury.Domain.Bank>(_UnitOfWork, _Repository, mapper , _provider)
    {

    }
}