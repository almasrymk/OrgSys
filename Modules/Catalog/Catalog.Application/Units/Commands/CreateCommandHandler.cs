namespace Catalog.Application.Units.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateUnitCommand: UnitDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Unit> _Repository , IMapper mapper) : CreateCommandHandler<CreateUnitCommand, Catalog.Domain.Unit>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
