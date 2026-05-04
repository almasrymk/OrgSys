namespace Application.Commands.Org.Setting.Account.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListAccountCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Account> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListAccountCommand, Entity.Model.Account>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Account, bool>> CreateFilter(DeleteListAccountCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}