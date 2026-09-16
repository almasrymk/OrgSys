namespace Inventory.Application.Stocks.Queries
{
    using Organization.Contracts.Branches;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdStockQuery(long Id) : ICommand<StockDto> , IGetByIdQuery<Result<StockDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.Stock> _Repository, IMapper mapper, ISender sender) : GetCommandHandler<GetByIdStockQuery, Inventory.Domain.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(GetByIdStockQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<Result<StockDto>> Handle(GetByIdStockQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response is null || result.Response.Id == 0)
                return result;

            var names = (await sender.Send(new GetBranchNamesQuery([result.Response.BranchId]), cancellationToken)).Response ?? [];
            result.Response.BranchName = names.GetValueOrDefault(result.Response.BranchId);
            return result;
        }
    }
}
