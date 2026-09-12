namespace Purchasing.Domain
{
    [Table("PurchaseOrderProduct")]
    public class PurchaseOrderProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("PurchaseOrder")]
        public virtual long PurchaseOrderId { get; set; }

        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        // Product navigation dropped — Purchasing/Catalog(Inventory) module boundary, same
        // convention as Sales.Domain.InvoiceProduct/OrderProduct.
        public virtual long ProductId { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit? Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; set; }
    }
}
