namespace Inventory.Application.Stocks.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListStockQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<StockDto> , IListQuery<ResultCollection<StockDto>>;

    public sealed class GetListQueryHandler(IRepository<Inventory.Domain.Stock> _Repository, IMapper mapper, ISender sender) : ListCommandHandler<GetListStockQuery, Inventory.Domain.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(GetListStockQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Inventory.Domain.Stock>, IOrderedQueryable<Inventory.Domain.Stock>> CreateOrderBy(GetListStockQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Branch";
        }

        public override async Task<ResultCollection<StockDto>> Handle(GetListStockQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            var accountIds = result.Response.Where(e => e.AccountId is > 0).Select(e => e.AccountId!.Value).Distinct().ToList();
            if (accountIds.Count > 0)
            {
                var names = (await sender.Send(new GetAccountNamesQuery(accountIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.AccountId is > 0 && names.TryGetValue(dto.AccountId.Value, out var name))
                        dto.AccountName = name;
            }

            return result;
        }
    }
}
