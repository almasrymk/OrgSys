namespace Application.Commands.Org.Setting.DealerGroup.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListDealerGroupCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.DealerGroup> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListDealerGroupCommand, Entity.Model.DealerGroup>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.DealerGroup, bool>> CreateFilter(DeleteListDealerGroupCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}