namespace Application.Commands.Org.Setting.User.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteUserCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.User> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteUserCommand, Entity.Model.User>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.User, bool>> CreateFilter(DeleteUserCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}