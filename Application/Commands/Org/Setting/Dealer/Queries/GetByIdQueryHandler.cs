namespace Application.Commands.Org.Setting.Dealer.Queries
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

    public sealed record GetByIdDealerQuery(long Id) : ICommand<DealerModelView> , IGetByIdQuery<Result<DealerModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Dealer> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDealerQuery, Entity.Model.Dealer, DealerModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Dealer, bool>> CreateFilter(GetByIdDealerQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}