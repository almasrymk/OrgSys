namespace Inventory.Domain
{
    /// <summary>One line of a StockAdjustment. No public constructor — only reachable through
    /// StockAdjustment.AddLine.</summary>
    [Table("StockAdjustmentLine")]
    public class StockAdjustmentLine : BaseModel
    {
        protected StockAdjustmentLine() { }

        internal StockAdjustmentLine(long productId, long unitId, MovementDirection direction, decimal quantity, long? batchId, string? notes)
        {
            if (quantity <= 0)
                throw new InventoryDocumentLineRequiredException("An adjustment line quantity must be greater than zero.");

            ProductId = productId;
            UnitId = unitId;
            Direction = direction;
            Quantity = quantity;
            BatchId = batchId;
            Notes = notes;
        }

        public virtual long RowNumber { get; internal set; }

        [ForeignKey(nameof(StockAdjustment))]
        public virtual long StockAdjustmentId { get; internal set; }

        public virtual StockAdjustment? StockAdjustment { get; set; }

        /// <summary>Scalar-only Catalog Product/Unit references. FKs preserved in OrgContext.</summary>
        public virtual long ProductId { get; private set; }

        public virtual long UnitId { get; private set; }

        public virtual MovementDirection Direction { get; private set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal Quantity { get; private set; }

        [ForeignKey("Batch")]
        public virtual long? BatchId { get; private set; }

        public virtual InventoryBatch? Batch { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }
    }
}
