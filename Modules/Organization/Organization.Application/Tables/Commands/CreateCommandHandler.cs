namespace Organization.Application.Tables.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateTableCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Table> _Repository , IMapper mapper) : CreateCommandHandler<CreateTableCommand, Organization.Domain.Table>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}