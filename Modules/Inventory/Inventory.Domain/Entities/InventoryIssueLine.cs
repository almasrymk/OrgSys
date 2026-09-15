namespace Inventory.Domain
{
    /// <summary>One line of an InventoryIssue. No public constructor — only reachable through
    /// InventoryIssue.AddLine.</summary>
    [Table("InventoryIssueLine")]
    public class InventoryIssueLine : BaseModel
    {
        protected InventoryIssueLine() { }

        internal InventoryIssueLine(long productId, long unitId, decimal quantity, long? batchId, long? serialId, string? notes)
        {
            if (quantity <= 0)
                throw new InventoryDocumentLineRequiredException("An issue line quantity must be greater than zero.");
            if (serialId.HasValue && quantity != 1)
                throw new InventoryDocumentLineRequiredException("A serial-tracked issue line must have a quantity of exactly 1.");

            ProductId = productId;
            UnitId = unitId;
            Quantity = quantity;
            BatchId = batchId;
            SerialId = serialId;
            Notes = notes;
        }

        public virtual long RowNumber { get; internal set; }

        [ForeignKey(nameof(InventoryIssue))]
        public virtual long InventoryIssueId { get; internal set; }

        public virtual InventoryIssue? InventoryIssue { get; set; }

        public virtual long ProductId { get; private set; }

        public virtual Product? Product { get; set; }

        public virtual long UnitId { get; private set; }

        public virtual Unit? Unit { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal Quantity { get; private set; }

        [ForeignKey("Batch")]
        public virtual long? BatchId { get; private set; }

        public virtual InventoryBatch? Batch { get; set; }

        [ForeignKey("Serial")]
        public virtual long? SerialId { get; private set; }

        public virtual InventorySerial? Serial { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }
    }
}
