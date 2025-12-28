namespace Application.Commands.Org.Setting.DealerGroup.Queries
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

    public sealed record GetByIdDealerGroupQuery(long Id) : ICommand<DealerGroupModelView> , IGetByIdQuery<Result<DealerGroupModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.DealerGroup> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDealerGroupQuery, Entity.Model.DealerGroup, DealerGroupModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.DealerGroup, bool>> CreateFilter(GetByIdDealerGroupQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}