namespace Application.Commands.Org.Setting.Safe.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateSafeCommand(long Id , long? SafeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Safe> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateSafeCommand, Entity.Model.Safe>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}