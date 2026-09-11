namespace MasterData.Application.Countries.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateCountryCommand(long Id , string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Country> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCountryCommand, MasterData.Domain.Country>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
