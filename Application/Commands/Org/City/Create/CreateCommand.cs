namespace Application.Commands.Org.City.Create
{
    using Application.Abstraction.Command;

    public sealed record CreateCommand(long? CountryId , string Name) : ICommand<CreateCommandResponse>;

    public sealed record CreateCommandResponse(Guid Id, long? CountryId, string Name);
}