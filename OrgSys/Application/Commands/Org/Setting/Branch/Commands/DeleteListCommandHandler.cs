namespace Application.Commands.Org.Setting.Branch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListBranchCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Branch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListBranchCommand, Entity.Model.Branch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Branch, bool>> CreateFilter(DeleteListBranchCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}