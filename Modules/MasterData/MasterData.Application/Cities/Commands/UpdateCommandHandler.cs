namespace MasterData.Application.Cities.Commands
{
    using AutoMapper;
    using OrgSys.SharedKernel;

    public sealed record UpdateCityCommand(long Id , long? CountryId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.City> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCityCommand, MasterData.Domain.City>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
