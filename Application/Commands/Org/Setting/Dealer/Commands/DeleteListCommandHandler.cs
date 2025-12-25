namespace Application.Commands.Org.Setting.Dealer.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListDealerCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Dealer> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListDealerCommand, Entity.Model.Dealer>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Dealer, bool>> CreateFilter(DeleteListDealerCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}