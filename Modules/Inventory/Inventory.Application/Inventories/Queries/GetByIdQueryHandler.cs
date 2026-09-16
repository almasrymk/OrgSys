namespace Inventory.Application.Inventories.Queries
{
    using Catalog.Contracts.Products;
    using Catalog.Contracts.Units;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdInventoryQuery(long Id) : ICommand<InventoryDto> , IGetByIdQuery<Result<InventoryDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.Inventory> _Repository, IRepository<Inventory.Domain.Transaction> transactionRepository, ISender sender, IMapper mapper) : GetCommandHandler<GetByIdInventoryQuery, Inventory.Domain.Inventory, InventoryDto>(_Repository, mapper)
    {
        public override async Task<Result<InventoryDto>> Handle(GetByIdInventoryQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response == null || result.Response.Id == 0)
                return result;

            var adjustments = await transactionRepository.GetListByFilterAsync(e =>
                e.InventoryId == result.Response.Id
                && (e.TypeId == 5 || e.TypeId == 6)
                && e.Status != OrgSys.SharedKernel.Status.Deleted
                && !e.Hide);
            var adjustmentIn = adjustments?.FirstOrDefault(e => e.TypeId == 5);
            var adjustmentOut = adjustments?.FirstOrDefault(e => e.TypeId == 6);
            result.Response.AdjustmentInTransactionId = adjustmentIn?.Id;
            result.Response.AdjustmentInTransactionCode = adjustmentIn?.Code;
            result.Response.AdjustmentOutTransactionId = adjustmentOut?.Id;
            result.Response.AdjustmentOutTransactionCode = adjustmentOut?.Code;
            result.Response.HasAdjustment = adjustmentIn != null || adjustmentOut != null;

            var lines = result.Response.InventoryProductList ?? [];
            if (lines.Count > 0)
            {
                var productIds = lines.Select(e => e.ProductId).Distinct().ToList();
                var unitIds = lines.Select(e => e.UnitId).Distinct().ToList();
                var productNames = (await sender.Send(new GetProductNamesQuery(productIds), cancellationToken)).Response ?? [];
                var unitNames = (await sender.Send(new GetUnitNamesQuery(unitIds), cancellationToken)).Response ?? [];
                foreach (var line in lines)
                {
                    line.ProductName = productNames.GetValueOrDefault(line.ProductId);
                    line.UnitName = unitNames.GetValueOrDefault(line.UnitId);
                }
            }
            return result;
        }

        public override string CreateInclude()
        {
            return "InventoryProducts";
        }

        public override Expression<Func<Inventory.Domain.Inventory, bool>> CreateFilter(GetByIdInventoryQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
