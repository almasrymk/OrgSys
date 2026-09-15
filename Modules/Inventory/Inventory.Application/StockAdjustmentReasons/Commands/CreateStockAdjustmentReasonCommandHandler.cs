namespace Inventory.Application.StockAdjustmentReasons.Commands;

public sealed record CreateStockAdjustmentReasonCommand(string Code, string Name) : ICommand, ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork unitOfWork, IRepository<StockAdjustmentReason> repository, AutoMapper.IMapper mapper)
    : CreateCommandHandler<CreateStockAdjustmentReasonCommand, StockAdjustmentReason>(unitOfWork, repository, mapper);
