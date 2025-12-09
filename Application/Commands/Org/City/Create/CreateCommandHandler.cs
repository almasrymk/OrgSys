namespace Application.Commands.Org.City.Create
{
    using Entity.Model;
    using Entity.ModelView;
    using Application.Common;
    using Domain.Abstraction;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository) : CreateCommandHandler<CreateCommand, Entity.Model.City , CityModelView>(_UnitOfWork, _Repository)
    {
        public override City GetMapModel(CreateCommand request)
        {
            return new City { CountryId = request.CountryId, Name = request.Name };
        }

        public override CityModelView GetMapResponse(City model)
        {
            return new CityModelView { Id = model.Id, Name = model.Name, CountryId = model.CountryId };
        }
    }
}