namespace Inventory.Application.Stocks.Queries
{
    using Accounting.Contracts.Accounts;
    using Organization.Contracts.Branches;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record SearchStockQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<StockDto> ,ISearchQuery<ResultPagination<StockDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.Stock> _Repository, IMapper mapper, ISender sender) : SearchCommandHandler<SearchStockQuery, Inventory.Domain.Stock, StockDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(SearchStockQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Inventory.Domain.Stock>, IOrderedQueryable<Inventory.Domain.Stock>> CreateOrderBy(SearchStockQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return string.Empty;
        }

        public override async Task<ResultPagination<StockDto>> Handle(SearchStockQuery request, CancellationToken cancellationToken)
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

            var branchIds = result.Response.Select(e => e.BranchId).Distinct().ToList();
            if (branchIds.Count > 0)
            {
                var names = (await sender.Send(new GetBranchNamesQuery(branchIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    dto.BranchName = names.GetValueOrDefault(dto.BranchId);
            }

            return result;
        }
    }
}
