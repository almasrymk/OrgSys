namespace MasterData.Application.Districts.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateDistrictCommand : DistrictDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.District> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateDistrictCommand, MasterData.Domain.District>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}
