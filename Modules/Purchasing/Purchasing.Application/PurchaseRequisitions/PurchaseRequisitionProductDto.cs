namespace Purchasing.Application
{
    /// <summary>Standalone DTO — see PurchaseOrderProductDto's remark for why this no longer
    /// inherits from Purchasing.Domain.PurchaseRequisitionProduct.</summary>
    public class PurchaseRequisitionProductDto
    {
        public long Id { get; set; }

        public long RowNumber { get; set; }

        public long PurchaseRequisitionId { get; set; }

        public long ProductId { get; set; }

        public long UnitId { get; set; }

        public decimal Quantity { get; set; }

        public decimal OrderedQuantity { get; set; }

        public string? Notes { get; set; }

        public string? ProductName { get; set; }

        public string? UnitName { get; set; }
    }
}
