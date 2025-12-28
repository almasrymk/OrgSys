namespace Application.Commands.Org.Setting.Classification.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateClassificationCommand(long Id , long? ClassificationId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Classification> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateClassificationCommand, Entity.Model.Classification>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}