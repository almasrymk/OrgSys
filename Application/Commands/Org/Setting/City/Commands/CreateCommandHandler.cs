namespace Application.Commands.Org.Setting.City.Commands
{
    using AutoMapper;
    using Domain.Shared;
    using Domain.Abstraction;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Application.Abstraction.Command;

    public sealed record CreateCityCommand (string? Name , long? CountryId) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.City> _Repository , IMapper mapper) : CreateCommandHandler<CreateCityCommand, Domain.Entities.City>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}