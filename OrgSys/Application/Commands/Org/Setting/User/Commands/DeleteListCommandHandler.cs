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

    public sealed record DeleteListUserCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.User> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListUserCommand, Entity.Model.User>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.User, bool>> CreateFilter(DeleteListUserCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}