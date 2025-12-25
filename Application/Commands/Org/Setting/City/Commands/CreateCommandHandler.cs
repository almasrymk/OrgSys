namespace Application.Commands.Org.Setting.City.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateCityCommand(long? CountryId, string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : CreateCommandHandler<CreateCityCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}