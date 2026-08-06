namespace Application.Commands.Org.Transactions.Inventory.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdInventoryQuery(long Id) : ICommand<InventoryDto> , IGetByIdQuery<Result<InventoryDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Inventory> _Repository, IRepository<Domain.Entities.Transaction> transactionRepository, IMapper mapper) : GetCommandHandler<GetByIdInventoryQuery, Domain.Entities.Inventory, InventoryDto>(_Repository, mapper)
    {
        public override async Task<Result<InventoryDto>> Handle(GetByIdInventoryQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response == null || result.Response.Id == 0)
                return result;

            var adjustments = await transactionRepository.GetListByFilterAsync(e =>
                e.InventoryId == result.Response.Id
                && (e.TypeId == 5 || e.TypeId == 6)
                && e.Status != Domain.Enums.Status.Deleted
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

        public override Expression<Func<Domain.Entities.Inventory, bool>> CreateFilter(GetByIdInventoryQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
