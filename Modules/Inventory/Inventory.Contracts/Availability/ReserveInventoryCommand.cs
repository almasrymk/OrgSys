namespace Inventory.Contracts.Availability;

using OrgSys.SharedKernel;

/// <summary>Contracts-local mirror of Inventory.Domain.SourceDocumentType's values — deliberately a
/// separate type, not a reference to the Domain enum, so a caller only ever needs a reference to
/// Inventory.Contracts (brief §43: Contracts must not expose Domain types; module-dependency-map.md
/// §1: other modules depend on Contracts only).</summary>
public enum ReservationSourceType
{
    SalesOrder = 8
}

/// <summary>
/// Public reservation contract for other modules (brief §14/§43) — e.g. Sales Order confirmation
/// reserving stock without Sales ever touching InventoryBalance directly. Wraps the same
/// concurrency-safe reserve flow ReserveStockCommand (Inventory.Application internal) uses.
/// </summary>
public sealed record ReserveInventoryCommand(
    long ProductId, long StockId, long? LocationId, long? BatchId, decimal Quantity,
    ReservationSourceType SourceType, long SourceId, long? SourceLineId, long CreateUserId) : ICommand<long>;
