namespace Application.Commands.Org.Setting.City.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateCityCommand(long Id , long? CountryId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateCityCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}