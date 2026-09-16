namespace Inventory.Domain
{
    /// <summary>One line of an InventoryReceipt. No public constructor — only reachable through
    /// InventoryReceipt.AddLine, mirroring Sales.Domain.QuotationLine's child-entity shape.</summary>
    [Table("InventoryReceiptLine")]
    public class InventoryReceiptLine : BaseModel
    {
        protected InventoryReceiptLine() { }

        internal InventoryReceiptLine(long productId, long unitId, decimal quantity, decimal unitCost, long? batchId, string? notes)
        {
            if (quantity <= 0)
                throw new InventoryDocumentLineRequiredException("A receipt line quantity must be greater than zero.");
            if (unitCost < 0)
                throw new InventoryDocumentLineRequiredException("A receipt line unit cost cannot be negative.");

            ProductId = productId;
            UnitId = unitId;
            Quantity = quantity;
            UnitCost = unitCost;
            BatchId = batchId;
            Notes = notes;
        }

        public virtual long RowNumber { get; internal set; }

        [ForeignKey(nameof(InventoryReceipt))]
        public virtual long InventoryReceiptId { get; internal set; }

        public virtual InventoryReceipt? InventoryReceipt { get; set; }

        /// <summary>Scalar-only Catalog Product/Unit references. FKs preserved in OrgContext.</summary>
        public virtual long ProductId { get; private set; }

        public virtual long UnitId { get; private set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal Quantity { get; private set; }

        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal UnitCost { get; private set; }

        [ForeignKey("Batch")]
        public virtual long? BatchId { get; private set; }

        public virtual InventoryBatch? Batch { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }
    }
}
