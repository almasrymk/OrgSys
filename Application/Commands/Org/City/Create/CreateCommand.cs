namespace Application.Commands.Org.City.Create
{
    using Application.Abstraction.Command;

    public sealed record CreateCommand(string Name) : ICommand<CreateCommandResponse>;

    public sealed record CreateCommandResponse(Guid Id, string Name);

}