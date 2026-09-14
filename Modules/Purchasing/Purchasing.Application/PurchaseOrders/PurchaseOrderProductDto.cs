namespace Purchasing.Application
{
    /// <summary>
    /// Standalone DTO — deliberately does NOT inherit from Purchasing.Domain.PurchaseOrderProduct
    /// (unlike before this hardening pass). PurchaseOrderProduct now has private setters and is only
    /// mutable through PurchaseOrder's own domain methods, so a DTO used for JSON (de)serialization
    /// must be a plain, independently-settable class. Field set is identical to what the old
    /// inherited DTO exposed, so the JSON wire shape is unchanged for existing callers.
    /// </summary>
    public class PurchaseOrderProductDto
    {
        public long Id { get; set; }

        public long RowNumber { get; set; }

        public long PurchaseOrderId { get; set; }

        public long ProductId { get; set; }

        public long UnitId { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public decimal ReceivedQuantity { get; set; }

        public decimal ReturnedQuantity { get; set; }

        public decimal CancelledQuantity { get; set; }

        public string? Notes { get; set; }

        public string? ProductName { get; set; }

        public string? UnitName { get; set; }
    }
}
