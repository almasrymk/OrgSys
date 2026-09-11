namespace Inventory.Application.Properties.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdatePropertyCommand(long Id , long? PropertyId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Property> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePropertyCommand, Inventory.Domain.Property>(_UnitOfWork, _Repository , mapper,_provider)
    {
       
    }
}