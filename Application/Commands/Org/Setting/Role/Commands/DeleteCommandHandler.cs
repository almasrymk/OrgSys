namespace Application.Commands.Org.Setting.Role.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteRoleCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Role> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteRoleCommand, Entity.Model.Role>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Role, bool>> CreateFilter(DeleteRoleCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}