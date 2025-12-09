namespace Application.Commands.Org.City.Create
{
    using Application.Abstraction.Command;
    using Entity.ModelView;

    public sealed record CreateCommand(long? CountryId , string Name) : ICommand<CityModelView>;
}   