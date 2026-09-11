namespace MasterData.Application.Countries.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateCountryCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Country> _Repository , IMapper mapper) : CreateCommandHandler<CreateCountryCommand, MasterData.Domain.Country>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
