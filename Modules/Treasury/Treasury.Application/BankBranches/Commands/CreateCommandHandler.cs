namespace Treasury.Application.BankBranches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateBankBranchCommand : BankBranchDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.BankBranch> _Repository , IMapper mapper) : CreateCommandHandler<CreateBankBranchCommand, Treasury.Domain.BankBranch>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}