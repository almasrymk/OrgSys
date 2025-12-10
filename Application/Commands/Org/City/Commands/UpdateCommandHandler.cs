namespace Application.Commands.Org.City.Commands
{
    using AutoMapper;
    using Domain.Abstraction;
    using Application.Abstraction.Command;
    using Application.Common.Commands;

    public sealed record UpdateCommand(long Id , long? CountryId, string Name) : ICommand;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}