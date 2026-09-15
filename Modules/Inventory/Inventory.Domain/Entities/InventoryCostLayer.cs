namespace Inventory.Domain
{
    /// <summary>
    /// One FIFO cost layer (brief §22) — created by a receipt movement, consumed oldest-first by
    /// issues against a FIFO-costed item. Only populated/consumed for items whose
    /// Product.CostingMethod is Fifo; WeightedAverage-costed items never create these rows and use
    /// InventoryBalance.AverageCost instead (brief §9 costing abstraction:
    /// FifoCostingStrategy vs WeightedAverageCostingStrategy).
    /// </summary>
    [Table("InventoryCostLayer")]
    public class InventoryCostLayer : BaseModel
    {
        protected InventoryCostLayer() { }

        [ForeignKey("Product")]
        public virtual long ProductId { get; private set; }

        public virtual Product? Product { get; set; }

        [ForeignKey("Stock")]
        public virtual long StockId { get; private set; }

        public virtual Stock? Stock { get; set; }

        [ForeignKey("Batch")]
        public virtual long? BatchId { get; private set; }

        public virtual InventoryBatch? Batch { get; set; }

        /// <summary>The InventoryMovement (Transaction) row that created this layer — reference id
        /// only, same module, so a plain FK is fine (not a cross-module reference).</summary>
        public virtual long ReceiptMovementId { get; private set; }

        public virtual DateTime ReceiptDate { get; private set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal OriginalQuantity { get; private set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal RemainingQuantity { get; private set; }

        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal UnitCost { get; private set; }

        [NotMapped]
        public bool IsDepleted => RemainingQuantity <= 0;

        public static InventoryCostLayer Create(long productId, long stockId, long? batchId, long receiptMovementId, DateTime receiptDate, decimal quantity, decimal unitCost)
        {
            if (quantity <= 0)
                throw new InsufficientStockException("A cost layer quantity must be greater than zero.");
            if (unitCost < 0)
                throw new InsufficientStockException("A cost layer unit cost cannot be negative.");

            return new InventoryCostLayer
            {
                ProductId = productId,
                StockId = stockId,
                BatchId = batchId,
                ReceiptMovementId = receiptMovementId,
                ReceiptDate = receiptDate,
                OriginalQuantity = quantity,
                RemainingQuantity = quantity,
                UnitCost = unitCost
            };
        }

        /// <summary>Consumes up to RemainingQuantity from this layer and returns how much was
        /// actually taken (may be less than requested if the layer doesn't have enough left) — the
        /// caller (FifoCostingStrategy) moves to the next-oldest layer for any shortfall. This never
        /// rewrites UnitCost — brief §22: a layer's cost is fixed at receipt time.</summary>
        public decimal Consume(decimal requestedQuantity)
        {
            if (requestedQuantity <= 0)
                throw new InsufficientStockException("A cost layer consumption quantity must be greater than zero.");

            var consumed = Math.Min(requestedQuantity, RemainingQuantity);
            RemainingQuantity -= consumed;
            return consumed;
        }
    }
}
