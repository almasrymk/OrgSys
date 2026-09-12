namespace Purchasing.Domain
{
    /// <summary>
    /// A confirmed order to a specific supplier — the Purchasing-owned counterpart to Sales'
    /// <c>Order</c>. Stays open (Status.New) until someone manually creates the resulting Purchase
    /// Invoice (Sales.Domain.Invoice, InvoiceType=Purchase) and links it here via
    /// LinkInvoiceCommand, at which point Status becomes Approved. No automatic Invoice generation
    /// is performed — see LinkInvoiceCommandHandler.
    /// </summary>
    [Table("PurchaseOrder")]
    public class PurchaseOrder : MovementModel
    {
        [ForeignKey("Dealer")]
        public virtual long DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; }

        /// <summary>Optional provenance link back to the requisition this order was raised from.</summary>
        public virtual long? PurchaseRequisitionId { get; set; }

        public virtual PurchaseRequisition? PurchaseRequisition { get; set; }

        /// <summary>Set once a Purchase Invoice has been created and manually linked — see
        /// LinkInvoiceCommand. No EF navigation to Sales.Domain.Invoice: resolved via
        /// Sales.Contracts/a batch lookup if a display name is ever needed, same boundary
        /// discipline as InvoiceProduct.ProductId.</summary>
        public virtual long? InvoiceId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Discount { get; set; }

        public virtual int DiscountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Tax { get; set; }

        public virtual int TaxType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Net { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; set; }

        public virtual ICollection<PurchaseOrderProduct>? PurchaseOrderProducts { get; set; }
    }
}
