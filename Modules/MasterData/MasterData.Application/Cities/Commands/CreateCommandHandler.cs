namespace MasterData.Application.Cities.Commands
{
    using AutoMapper;
    using OrgSys.SharedKernel;

    public sealed record CreateCityCommand (string? Name , long? CountryId) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.City> _Repository , IMapper mapper) : CreateCommandHandler<CreateCityCommand, MasterData.Domain.City>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
