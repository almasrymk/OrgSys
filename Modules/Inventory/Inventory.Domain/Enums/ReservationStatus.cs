namespace Inventory.Domain.Enums;

/// <summary>StockReservation's lifecycle (brief §14). A reservation never mutates OnHand — only
/// QuantityReserved on InventoryBalance — so these states track the reservation's own fate, not
/// physical stock.</summary>
public enum ReservationStatus
{
    Active = 0,
    Released = 10,
    Fulfilled = 20,
    Expired = 30,
    Cancelled = 40
}
