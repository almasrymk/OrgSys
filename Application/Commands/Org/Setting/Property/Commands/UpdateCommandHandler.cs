namespace Application.Commands.Org.Setting.Property.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdatePropertyCommand(long Id , long? PropertyId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Property> _Repository , IMapper mapper) : UpdateCommandHandler<UpdatePropertyCommand, Entity.Model.Property>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}