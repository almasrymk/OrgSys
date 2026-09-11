namespace Inventory.Application.Inventories.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchInventoryQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<InventoryDto> ,ISearchQuery<ResultPagination<InventoryDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.Inventory> _Repository,
        IRepository<Inventory.Domain.Transaction> transactionRepository, IMapper mapper) : SearchCommandHandler<SearchInventoryQuery, Inventory.Domain.Inventory, InventoryDto>(_Repository, mapper)
    {
        public override async Task<ResultPagination<InventoryDto>> Handle(SearchInventoryQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var inventoryIds = result.Response.Select(e => e.Id).ToList();
            if (inventoryIds.Count == 0)
                return result;

            var adjustments = await transactionRepository.GetListByFilterAsync(e =>
                e.InventoryId.HasValue
                && inventoryIds.Contains(e.InventoryId.Value)
                && (e.TypeId == 5 || e.TypeId == 6)
                && e.Status != OrgSys.SharedKernel.Status.Deleted
                && e.Hide != true);

            foreach (var inventory in result.Response)
            {
                var adjustmentIn = adjustments?.FirstOrDefault(e => e.InventoryId == inventory.Id && e.TypeId == 5);
                var adjustmentOut = adjustments?.FirstOrDefault(e => e.InventoryId == inventory.Id && e.TypeId == 6);
                inventory.AdjustmentInTransactionId = adjustmentIn?.Id;
                inventory.AdjustmentInTransactionCode = adjustmentIn?.Code;
                inventory.AdjustmentOutTransactionId = adjustmentOut?.Id;
                inventory.AdjustmentOutTransactionCode = adjustmentOut?.Code;
                inventory.HasAdjustment = inventory.AdjustmentInTransactionId > 0 || inventory.AdjustmentOutTransactionId > 0;
            }

            return result;
        }

        public override Expression<Func<Inventory.Domain.Inventory, bool>> CreateFilter(SearchInventoryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Stock";
        }

        override public Func<IQueryable<Inventory.Domain.Inventory>, IOrderedQueryable<Inventory.Domain.Inventory   >> CreateOrderBy(SearchInventoryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
