namespace Purchasing.Domain
{
    /// <summary>
    /// One line within a PurchaseOrder. ProductId is a scalar reference (navigation dropped, same
    /// convention as PurchaseRequisitionProduct/InvoiceProduct/OrderProduct); UnitId keeps its
    /// existing EF navigation to MasterData.Unit. Total is domain-calculated (Quantity * Price) —
    /// brief §27 "Order totals calculated in domain" — rather than trusted from the caller as
    /// before hardening. ReceivedQuantity/ReturnedQuantity/CancelledQuantity are this line's own
    /// authoritative running totals of physical fulfillment so far (brief §33/§86), maintained only
    /// through RecordReceipt/RecordReturn/CancelPartial — the same relationship
    /// Sales.Domain.SalesOrderLine's Delivered/Returned/Cancelled quantities have to SalesOrder;
    /// actual Inventory stock-movement integration remains a separate, later phase. No public
    /// constructor — only reachable through PurchaseOrder.AddLine.
    /// </summary>
    [Table("PurchaseOrderProduct")]
    public class PurchaseOrderProduct : BaseModel
    {
        /// <summary>EF materialization constructor only.</summary>
        protected PurchaseOrderProduct() { }

        internal PurchaseOrderProduct(long rowNumber, long productId, long unitId, decimal quantity, decimal price, string? notes)
        {
            RowNumber = rowNumber;
            ProductId = productId;
            UnitId = unitId;
            Quantity = quantity;
            Price = price;
            Notes = notes;
            RecalculateTotal();
        }

        [ForeignKey("PurchaseOrder")]
        public virtual long PurchaseOrderId { get; internal set; }

        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        [Required]
        public virtual long RowNumber { get; private set; }

        // Product navigation dropped — Purchasing/Catalog(Inventory) module boundary, same
        // convention as CommercialDocuments.Domain.InvoiceProduct/Sales.Domain.OrderProduct.
        public virtual long ProductId { get; private set; }

        /// <summary>Scalar-only Catalog Unit reference. FK preserved in OrgContext.</summary>
        public virtual long UnitId { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal ReceivedQuantity { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal ReturnedQuantity { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal CancelledQuantity { get; private set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }

        /// <summary>Ordered minus received minus cancelled (brief §86) — what could still be received on this line.</summary>
        public decimal RemainingQuantity => Quantity - ReceivedQuantity - CancelledQuantity;

        /// <summary>Received minus already returned (brief §47) — the maximum a further return on this line can take.</summary>
        public decimal ReturnableQuantity => ReceivedQuantity - ReturnedQuantity;

        internal void RecordReceipt(decimal quantity)
        {
            if (quantity > RemainingQuantity)
                throw new InvalidPurchaseOrderReceiptException(
                    $"Cannot receive {quantity} on line for product {ProductId} — only {RemainingQuantity} remains.");

            ReceivedQuantity += quantity;
        }

        internal void RecordReturn(decimal quantity)
        {
            if (quantity > ReturnableQuantity)
                throw new InvalidPurchaseOrderReturnException(
                    $"Cannot return {quantity} on line for product {ProductId} — only {ReturnableQuantity} of the received quantity has not already been returned.");

            ReturnedQuantity += quantity;
        }

        /// <summary>Cancels part (or, called with RemainingQuantity, all) of this line's remaining
        /// (unreceived, uncancelled) quantity. Never reduces Quantity or received history.</summary>
        internal void CancelPartial(decimal quantity)
        {
            if (quantity > RemainingQuantity)
                throw new InvalidPurchaseOrderLineException(
                    $"Cannot cancel {quantity} on line for product {ProductId} — only {RemainingQuantity} remains.");

            CancelledQuantity += quantity;
        }

        internal void UpdateCommercial(long productId, long unitId, decimal quantity, decimal price, string? notes)
        {
            ProductId = productId;
            UnitId = unitId;
            Quantity = quantity;
            Price = price;
            Notes = notes;
            RecalculateTotal();
        }

        private void RecalculateTotal() => Total = Quantity * Price;
    }
}
