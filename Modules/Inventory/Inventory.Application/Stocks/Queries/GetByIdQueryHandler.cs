namespace Inventory.Application.Stocks.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdStockQuery(long Id) : ICommand<StockDto> , IGetByIdQuery<Result<StockDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.Stock> _Repository, IMapper mapper) : GetCommandHandler<GetByIdStockQuery, Inventory.Domain.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(GetByIdStockQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}