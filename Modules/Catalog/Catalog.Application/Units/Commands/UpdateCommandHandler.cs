namespace Catalog.Application.Units.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateUnitCommand : UnitDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Unit> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateUnitCommand, Catalog.Domain.Unit>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
