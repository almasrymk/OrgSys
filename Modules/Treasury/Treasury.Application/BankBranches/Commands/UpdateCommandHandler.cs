namespace Treasury.Application.BankBranches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateBankBranchCommand : BankBranchDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.BankBranch> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateBankBranchCommand, Treasury.Domain.BankBranch>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}