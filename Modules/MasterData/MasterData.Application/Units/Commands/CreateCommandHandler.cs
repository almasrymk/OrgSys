namespace MasterData.Application.Units.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateUnitCommand: UnitDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Unit> _Repository , IMapper mapper) : CreateCommandHandler<CreateUnitCommand, MasterData.Domain.Unit>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
