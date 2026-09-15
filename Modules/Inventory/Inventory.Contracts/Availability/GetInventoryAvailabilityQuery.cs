namespace Inventory.Contracts.Availability;

using OrgSys.SharedKernel;

/// <summary>
/// The public stock-availability contract (brief §42) — Sales/Purchasing/other modules call this
/// instead of ever reaching into Inventory's DbContext/entities directly. Handled by
/// Inventory.Application.
/// </summary>
public sealed record GetInventoryAvailabilityQuery(
    long ProductId, long StockId, long? LocationId, long? BatchId, decimal RequestedQuantity) : IQuery<InventoryAvailabilityDto>;

public sealed record InventoryAvailabilityDto(decimal OnHand, decimal Reserved, decimal Available, bool CanFulfill);
