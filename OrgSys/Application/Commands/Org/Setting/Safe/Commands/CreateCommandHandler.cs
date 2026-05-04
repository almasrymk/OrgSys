namespace Application.Commands.Org.Setting.Safe.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreateSafeCommand : SafeModelView , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Safe> _Repository , IMapper mapper) : CreateCommandHandler<CreateSafeCommand, Entity.Model.Safe>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}