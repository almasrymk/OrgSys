namespace Inventory.Domain
{
    /// <summary>One line of a StockTransfer. No public constructor — only reachable through
    /// StockTransfer.AddLine.</summary>
    [Table("StockTransferLine")]
    public class StockTransferLine : BaseModel
    {
        protected StockTransferLine() { }

        internal StockTransferLine(long productId, long unitId, decimal quantity, long? batchId, string? notes)
        {
            if (quantity <= 0)
                throw new InventoryDocumentLineRequiredException("A transfer line quantity must be greater than zero.");

            ProductId = productId;
            UnitId = unitId;
            Quantity = quantity;
            BatchId = batchId;
            Notes = notes;
        }

        public virtual long RowNumber { get; internal set; }

        [ForeignKey(nameof(StockTransfer))]
        public virtual long StockTransferId { get; internal set; }

        public virtual StockTransfer? StockTransfer { get; set; }

        public virtual long ProductId { get; private set; }

        public virtual Product? Product { get; set; }

        public virtual long UnitId { get; private set; }

        public virtual Unit? Unit { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal Quantity { get; private set; }

        [ForeignKey("Batch")]
        public virtual long? BatchId { get; private set; }

        public virtual InventoryBatch? Batch { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }
    }
}
