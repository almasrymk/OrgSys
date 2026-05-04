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

    public sealed record DeleteListRoleCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Role> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListRoleCommand, Entity.Model.Role>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Role, bool>> CreateFilter(DeleteListRoleCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}