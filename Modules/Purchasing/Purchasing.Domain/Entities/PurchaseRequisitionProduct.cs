namespace Purchasing.Domain
{
    /// <summary>
    /// One line within a PurchaseRequisition. ProductId is a scalar reference to Inventory's
    /// product catalog — navigation deliberately dropped, same convention as
    /// CommercialDocuments.Domain.InvoiceProduct/Sales.Domain.OrderProduct. UnitId keeps its
    /// existing EF navigation to MasterData.Unit (unchanged, existing accepted exception).
    /// OrderedQuantity is this line's own running total of how much has been sourced into a
    /// PurchaseOrder so far — maintained only through PurchaseRequisition.RecordSourced, laying the
    /// groundwork for partial sourcing (brief §13) once RFQ exists, without changing today's
    /// whole-requisition conversion behavior. No public constructor — only reachable through
    /// PurchaseRequisition.AddLine, mirroring Sales.Domain.QuotationLine.
    /// </summary>
    [Table("PurchaseRequisitionProduct")]
    public class PurchaseRequisitionProduct : BaseModel
    {
        /// <summary>EF materialization constructor only.</summary>
        protected PurchaseRequisitionProduct() { }

        internal PurchaseRequisitionProduct(long rowNumber, long productId, long unitId, decimal quantity, string? notes)
        {
            RowNumber = rowNumber;
            ProductId = productId;
            UnitId = unitId;
            Quantity = quantity;
            Notes = notes;
        }

        [ForeignKey("PurchaseRequisition")]
        public virtual long PurchaseRequisitionId { get; internal set; }

        public virtual PurchaseRequisition? PurchaseRequisition { get; set; }

        [Required]
        public virtual long RowNumber { get; private set; }

        // Product navigation dropped — Purchasing/Catalog(Inventory) module boundary, same
        // convention as CommercialDocuments.Domain.InvoiceProduct/Sales.Domain.OrderProduct.
        public virtual long ProductId { get; private set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; private set; }

        public virtual Unit? Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; private set; }

        /// <summary>How much of this line has already been sourced into a PurchaseOrder. Never
        /// assigned directly — see RecordOrdered.</summary>
        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal OrderedQuantity { get; private set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }

        /// <summary>Requested minus already-ordered — what could still be sourced on this line.</summary>
        public decimal RemainingQuantity => Quantity - OrderedQuantity;

        internal void RecordOrdered(decimal quantity)
        {
            if (quantity > RemainingQuantity)
                throw new InvalidPurchaseRequisitionLineException(
                    $"Cannot source {quantity} on line for product {ProductId} — only {RemainingQuantity} remains unordered.");

            OrderedQuantity += quantity;
        }
    }
}
