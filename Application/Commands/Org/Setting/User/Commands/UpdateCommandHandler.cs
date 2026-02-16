namespace Application.Commands.Org.Setting.User.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateUserCommand : UserModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.User> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateUserCommand, Entity.Model.User>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}