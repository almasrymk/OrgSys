namespace Application.Commands.Org.Setting.Country.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record CreateCountryCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Country> _Repository , IMapper mapper) : CreateCommandHandler<CreateCountryCommand, Domain.Entities.Country>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}