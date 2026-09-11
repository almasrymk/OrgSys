namespace Inventory.Application.Inventories.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdInventoryQuery(long Id) : ICommand<InventoryDto> , IGetByIdQuery<Result<InventoryDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.Inventory> _Repository, IRepository<Inventory.Domain.Transaction> transactionRepository, IMapper mapper) : GetCommandHandler<GetByIdInventoryQuery, Inventory.Domain.Inventory, InventoryDto>(_Repository, mapper)
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
            return result;
        }

        public override string CreateInclude()
        {
            return "InventoryProducts,InventoryProducts.Unit,InventoryProducts.Product";
        }

        public override Expression<Func<Inventory.Domain.Inventory, bool>> CreateFilter(GetByIdInventoryQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
