namespace Application.Commands.Org.Setting.Unit.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateUnitCommand : UnitModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Unit> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateUnitCommand, Entity.Model.Unit>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}