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

    public sealed record DeleteDealerGroupCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.DealerGroup> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteDealerGroupCommand, Entity.Model.DealerGroup>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.DealerGroup, bool>> CreateFilter(DeleteDealerGroupCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}