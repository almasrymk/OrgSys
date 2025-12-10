namespace Application.Commands.Org.City.Commands
{
    using AutoMapper;
    using Domain.Abstraction;
    using Application.Common.Commands;
    using Application.Abstraction.Command;

    public sealed record CreateCommand(long? CountryId, string Name) : ICommand;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : CreateCommandHandler<CreateCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}