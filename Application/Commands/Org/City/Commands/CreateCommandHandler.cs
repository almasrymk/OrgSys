namespace Application.Commands.Org.City.Commands
{
    using AutoMapper;
    using Entity.ModelView;
    using Domain.Abstraction;
    using Application.Abstraction.Command;
    using Application.Common.Commands;

    public sealed record CreateCommand(long? CountryId, string Name) : ICommand<CityModelView>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : CreateCommandHandler<CreateCommand, Entity.Model.City , CityModelView>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}