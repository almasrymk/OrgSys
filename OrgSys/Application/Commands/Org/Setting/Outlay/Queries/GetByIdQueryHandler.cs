namespace Application.Commands.Org.Setting.Outlay.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdOutlayQuery(long Id) : ICommand<OutlayModelView> , IGetByIdQuery<Result<OutlayModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Outlay> _Repository, IMapper mapper) : GetCommandHandler<GetByIdOutlayQuery, Entity.Model.Outlay, OutlayModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Outlay, bool>> CreateFilter(GetByIdOutlayQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}