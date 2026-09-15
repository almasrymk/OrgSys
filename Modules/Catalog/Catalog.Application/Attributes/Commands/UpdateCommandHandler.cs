namespace Catalog.Application.Attributes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdatePropertyCommand(long Id , long? PropertyId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Property> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePropertyCommand, Catalog.Domain.Property>(_UnitOfWork, _Repository , mapper,_provider)
    {
       
    }
}