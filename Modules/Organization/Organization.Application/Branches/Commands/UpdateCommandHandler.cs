namespace Organization.Application.Branches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateBranchCommand(long Id , string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Branch> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateBranchCommand, Organization.Domain.Branch>(_UnitOfWork, _Repository , mapper, _provider)
    {
       
    }
}