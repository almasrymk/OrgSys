namespace Inventory.Application.Availability;

using Inventory.Contracts.Availability;
using Inventory.Domain.Repositories;
using System.Net;

public sealed class GetInventoryAvailabilityQueryHandler(IInventoryBalanceRepository balanceRepository)
    : IQueryHandler<GetInventoryAvailabilityQuery, InventoryAvailabilityDto>
{
    public async Task<Result<InventoryAvailabilityDto>> Handle(GetInventoryAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var balance = await balanceRepository.GetReadOnlyAsync(request.ProductId, request.StockId, request.LocationId, request.BatchId, cancellationToken);

        var onHand = balance?.QuantityOnHand ?? 0;
        var reserved = balance?.QuantityReserved ?? 0;
        var available = onHand - reserved;

        var dto = new InventoryAvailabilityDto(onHand, reserved, available, available >= request.RequestedQuantity);
        return new Result<InventoryAvailabilityDto>(HttpStatusCode.OK, dto, null);
    }
}
