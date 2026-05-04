namespace Application.Commands.Org.Setting.Country.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateCountryCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Country> _Repository , IMapper mapper) : CreateCommandHandler<CreateCountryCommand, Entity.Model.Country>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}