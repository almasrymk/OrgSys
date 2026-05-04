namespace Application.Commands.Org.Setting.Property.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreatePropertyCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Property> _Repository , IMapper mapper) : CreateCommandHandler<CreatePropertyCommand, Entity.Model.Property>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}