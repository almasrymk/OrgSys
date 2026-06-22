namespace Application.Commands.Org.Setting.Country.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateCountryCommand(long Id , string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Country> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCountryCommand, Domain.Entities.Country>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}