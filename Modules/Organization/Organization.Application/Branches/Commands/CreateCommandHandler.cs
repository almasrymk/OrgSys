namespace Organization.Application.Branches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateBranchCommand(string Name, long CompanyId) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Branch> _Repository , IMapper mapper) : CreateCommandHandler<CreateBranchCommand, Organization.Domain.Branch>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}