namespace Application.Commands.Org.Setting.City.Commands
{
    using AutoMapper;
    using Domain.Shared;
    using Domain.Abstraction;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Application.Abstraction.Command;

    public sealed record UpdateCityCommand(long Id , long? CountryId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.City> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCityCommand, Domain.Entities.City>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}