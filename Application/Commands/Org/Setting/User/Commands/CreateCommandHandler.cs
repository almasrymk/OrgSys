namespace Application.Commands.Org.Setting.User.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreateUserCommand : UserModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.User> _Repository , IMapper mapper) : CreateCommandHandler<CreateUserCommand, Entity.Model.User>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}