namespace Application.Commands.Org.Setting.Classification.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateClassificationCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Classification> _Repository , IMapper mapper) : CreateCommandHandler<CreateClassificationCommand, Entity.Model.Classification>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}