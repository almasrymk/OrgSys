namespace MasterData.Application.Districts.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateDistrictCommand: DistrictDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.District> _Repository , IMapper mapper) : CreateCommandHandler<CreateDistrictCommand, MasterData.Domain.District>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
