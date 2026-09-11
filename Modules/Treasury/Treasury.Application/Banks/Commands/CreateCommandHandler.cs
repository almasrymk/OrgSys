namespace Treasury.Application.Banks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateBankCommand : BankDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Bank> _Repository, IMapper mapper) : CreateCommandHandler<CreateBankCommand, Treasury.Domain.Bank>(_UnitOfWork, _Repository, mapper)
    {

    }
}