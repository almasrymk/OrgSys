namespace Application.Commands.Org.Setting.Safe.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateSafeCommand : SafeModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Safe> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateSafeCommand, Entity.Model.Safe>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}