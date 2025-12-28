namespace Application.Commands.Org.Setting.Unit.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateUnitCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Unit> _Repository , IMapper mapper) : CreateCommandHandler<CreateUnitCommand, Entity.Model.Unit>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}