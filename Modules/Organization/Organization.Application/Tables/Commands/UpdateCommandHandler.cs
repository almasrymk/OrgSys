namespace Organization.Application.Tables.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateTableCommand(long Id , long? TableId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Table> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTableCommand, Organization.Domain.Table>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}