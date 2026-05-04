namespace Application.Commands.Org.Setting.Stock.Queries
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

    public sealed record GetByIdStockQuery(long Id) : ICommand<StockModelView> , IGetByIdQuery<Result<StockModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Stock> _Repository, IMapper mapper) : GetCommandHandler<GetByIdStockQuery, Entity.Model.Stock, StockModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Stock, bool>> CreateFilter(GetByIdStockQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}