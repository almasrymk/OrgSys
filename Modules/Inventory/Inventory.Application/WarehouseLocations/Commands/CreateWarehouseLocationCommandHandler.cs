namespace Inventory.Application.WarehouseLocations.Commands;

public sealed record CreateWarehouseLocationCommand(long StockId, string Code, string? Name, long? ParentLocationId, string? LocationType, bool IsReceivable, bool IsPickable)
    : ICommand, ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork unitOfWork, IRepository<WarehouseLocation> repository, AutoMapper.IMapper mapper)
    : CreateCommandHandler<CreateWarehouseLocationCommand, WarehouseLocation>(unitOfWork, repository, mapper);
