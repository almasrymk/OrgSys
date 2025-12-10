namespace Application.Commands.Org.City.Commands
{
    using AutoMapper;
    using Domain.Abstraction;
    using Application.Abstraction.Command;
    using Application.Common.Commands;

    public sealed record DeleteCommand(long Id , string Name) : ICommand;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}