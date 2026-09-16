namespace Inventory.Domain
{
    /// <summary>
    /// Optimized current-state projection over InventoryMovement (Transaction/TransactionProduct) —
    /// brief §8/§70. Transaction/TransactionProduct stay the ledger source of truth; InventoryBalance
    /// is reconstructable from them (see the backfill/reconciliation plan in
    /// docs/ddd/inventory-target-architecture.md §13) and is the only thing anything ever
    /// concurrency-checks against, since summing the whole ledger per request is not a lock point.
    /// Every mutation goes through this class's methods — nothing sets QuantityOnHand/QuantityReserved
    /// directly (brief §8: "Do not permit random handlers to modify this table"). RowVersion is a
    /// SQL Server rowversion concurrency token — the first optimistic-concurrency pattern in OrgSys
    /// (none existed before this; see docs/ddd/inventory-current-state.md §2), reused by every write
    /// path (receipt/issue/adjustment/transfer posting and reservation) per
    /// docs/ddd/inventory-target-architecture.md §8.
    /// </summary>
    [Table("InventoryBalance")]
    public class InventoryBalance : BaseModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        /// <summary>EF materialization constructor only. Business code creates a balance row through
        /// <see cref="Create"/> — the first Receive/Reserve against a never-before-seen
        /// Product+Stock(+Location)(+Batch) combination creates it with zeros, mirroring how the
        /// existing Product-balance query implicitly treats "no transactions yet" as zero.</summary>
        protected InventoryBalance() { }

        /// <summary>Scalar-only Catalog Product reference. FK preserved in OrgContext.</summary>
        public virtual long ProductId { get; private set; }

        [ForeignKey("Stock")]
        public virtual long StockId { get; private set; }

        public virtual Stock? Stock { get; set; }

        [ForeignKey("Location")]
        public virtual long? LocationId { get; private set; }

        public virtual WarehouseLocation? Location { get; set; }

        [ForeignKey("Batch")]
        public virtual long? BatchId { get; private set; }

        public virtual InventoryBatch? Batch { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal QuantityOnHand { get; private set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal QuantityReserved { get; private set; }

        /// <summary>Derived, never stored directly — brief §8's invariant (Available = OnHand -
        /// Reserved) is enforced purely by never letting QuantityReserved exceed QuantityOnHand in
        /// the mutation methods below, so this can't drift.</summary>
        [NotMapped]
        public decimal QuantityAvailable => QuantityOnHand - QuantityReserved;

        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal AverageCost { get; private set; }

        [NotMapped]
        public decimal InventoryValue => QuantityOnHand * AverageCost;

        /// <summary>SQL Server rowversion — EF maps [Timestamp] to a concurrency token automatically.</summary>
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public static InventoryBalance Create(long productId, long stockId, long? locationId, long? batchId) =>
            new()
            {
                ProductId = productId,
                StockId = stockId,
                LocationId = locationId,
                BatchId = batchId,
                QuantityOnHand = 0,
                QuantityReserved = 0,
                AverageCost = 0
            };

        /// <summary>
        /// Applies a positive movement (Receipt/TransferReceipt/AdjustmentIncrease/OpeningBalance)
        /// and recomputes the weighted-average cost (brief §21):
        /// ((OldQty×OldCost)+(ReceiptQty×ReceiptCost))/(OldQty+ReceiptQty). Costs already posted on
        /// prior movements are never rewritten — only this running average changes, and only forward.
        /// </summary>
        public void Receive(decimal quantity, decimal unitCost)
        {
            if (quantity <= 0)
                throw new InsufficientStockException("A receive quantity must be greater than zero.");

            var oldQuantity = QuantityOnHand;
            var oldCost = AverageCost;
            var newQuantity = oldQuantity + quantity;

            var newAverageCost = newQuantity == 0
                ? 0
                : ((oldQuantity * oldCost) + (quantity * unitCost)) / newQuantity;

            QuantityOnHand = newQuantity;
            if (newAverageCost != oldCost)
            {
                AverageCost = newAverageCost;
                _domainEvents.Add(new InventoryCostChangedDomainEvent(ProductId, StockId, oldCost, newAverageCost));
            }
        }

        /// <summary>
        /// Applies a negative movement (Issue/TransferIssue/AdjustmentDecrease/DamageLoss). Checks
        /// Available (OnHand - Reserved), not just OnHand, so an ordinary issue can never silently
        /// consume stock another reservation is holding (brief §14) — fulfilling a specific
        /// reservation goes through <see cref="FulfillReservation"/> instead, which is the only path
        /// allowed to draw down reserved stock.
        /// </summary>
        public void IssueOut(decimal quantity, bool allowNegativeStock)
        {
            if (quantity <= 0)
                throw new InsufficientStockException("An issue quantity must be greater than zero.");
            if (!allowNegativeStock && quantity > QuantityAvailable)
                throw new InsufficientStockException(
                    $"Requested quantity {quantity} exceeds available quantity {QuantityAvailable} for product {ProductId} at warehouse {StockId}.");

            QuantityOnHand -= quantity;
        }

        /// <summary>Reserves against Available, never against OnHand directly (brief §14/§15) — the
        /// caller (ReserveStockCommandHandler) is responsible for the optimistic-concurrency
        /// retry-on-conflict loop using RowVersion (docs/ddd/inventory-target-architecture.md §8);
        /// this method only enforces the business invariant once a consistent read is in hand.</summary>
        public void Reserve(decimal quantity, bool allowNegativeStock)
        {
            if (quantity <= 0)
                throw new InsufficientStockException("A reservation quantity must be greater than zero.");
            if (!allowNegativeStock && quantity > QuantityAvailable)
                throw new InsufficientStockException(
                    $"Requested reservation {quantity} exceeds available quantity {QuantityAvailable} for product {ProductId} at warehouse {StockId}.");

            QuantityReserved += quantity;
        }

        public void ReleaseReservation(decimal quantity)
        {
            if (quantity <= 0)
                throw new InsufficientStockException("A release quantity must be greater than zero.");
            if (quantity > QuantityReserved)
                throw new InsufficientStockException(
                    $"Cannot release {quantity} — only {QuantityReserved} is currently reserved for product {ProductId} at warehouse {StockId}.");

            QuantityReserved -= quantity;
        }

        /// <summary>Fulfillment physically removes the previously-reserved stock — OnHand and
        /// Reserved both drop by the same amount, so Available is unaffected by this call (it was
        /// already reduced when the reservation was made).</summary>
        public void FulfillReservation(decimal quantity)
        {
            if (quantity <= 0)
                throw new InsufficientStockException("A fulfillment quantity must be greater than zero.");
            if (quantity > QuantityReserved)
                throw new InsufficientStockException(
                    $"Cannot fulfill {quantity} — only {QuantityReserved} is currently reserved for product {ProductId} at warehouse {StockId}.");

            QuantityReserved -= quantity;
            QuantityOnHand -= quantity;
        }
    }
}
