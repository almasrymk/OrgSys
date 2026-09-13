namespace Purchasing.Domain
{
    [Table("PurchaseRequisitionProduct")]
    public class PurchaseRequisitionProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("PurchaseRequisition")]
        public virtual long PurchaseRequisitionId { get; set; }

        public virtual PurchaseRequisition? PurchaseRequisition { get; set; }

        // Product navigation dropped — Purchasing/Catalog(Inventory) module boundary, same
        // convention as CommercialDocuments.Domain.InvoiceProduct/Sales.Domain.OrderProduct.
        public virtual long ProductId { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit? Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; set; }
    }
}
