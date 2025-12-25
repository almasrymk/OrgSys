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

    public sealed record DeleteDealerCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Dealer> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteDealerCommand, Entity.Model.Dealer>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Dealer, bool>> CreateFilter(DeleteDealerCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}