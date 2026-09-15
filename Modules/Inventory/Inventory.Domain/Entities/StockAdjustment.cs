namespace Inventory.Domain
{
    /// <summary>
    /// A manual stock adjustment (brief §12) — distinct from the StockCount-triggered adjustment
    /// (the existing Inventory/InventoryProduct + InventoryAdjustmentIntegration flow, kept as-is,
    /// see docs/ddd/inventory-current-state.md §4). Use this aggregate for a standalone
    /// damage/loss/found/correction that isn't the output of a full physical count. Both paths post
    /// through the same InventoryMovement pipeline (MovementType.AdjustmentIncrease/Decrease) so
    /// there is still exactly one ledger, never two competing adjustment code paths.
    /// </summary>
    [Table("StockAdjustment")]
    public class StockAdjustment : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        private readonly List<StockAdjustmentLine> _lines = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        public virtual IReadOnlyCollection<StockAdjustmentLine> Lines => _lines.AsReadOnly();

        protected StockAdjustment() { }

        [ForeignKey("Stock")]
        public virtual long StockId { get; private set; }

        public virtual Stock? Stock { get; set; }

        [ForeignKey("Reason")]
        public virtual long ReasonId { get; private set; }

        public virtual StockAdjustmentReason? Reason { get; set; }

        public virtual DocumentStatus LifecycleStatus { get; private set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }

        public static StockAdjustment Create(long stockId, long reasonId, DateTime date, long createUserId, DateTime createDate, long? branchId, string? notes)
        {
            if (stockId <= 0)
                throw new InventoryDocumentLineRequiredException("An adjustment must target a valid warehouse.");
            if (reasonId <= 0)
                throw new InventoryDocumentLineRequiredException("An adjustment must specify a reason.");

            return new StockAdjustment
            {
                StockId = stockId,
                ReasonId = reasonId,
                Date = date,
                CreateUserId = createUserId,
                CreateDate = createDate,
                BranchId = branchId,
                Notes = notes,
                LifecycleStatus = DocumentStatus.Draft
            };
        }

        public StockAdjustmentLine AddLine(long productId, long unitId, MovementDirection direction, decimal quantity, long? batchId, string? notes)
        {
            EnsureDraft();
            var line = new StockAdjustmentLine(productId, unitId, direction, quantity, batchId, notes)
            {
                StockAdjustment = this,
                StockAdjustmentId = Id,
                RowNumber = _lines.Count + 1
            };
            _lines.Add(line);
            return line;
        }

        public void RemoveLine(StockAdjustmentLine line)
        {
            EnsureDraft();
            _lines.Remove(line);
        }

        public void Confirm()
        {
            EnsureDraft();
            EnsureHasLines();
            LifecycleStatus = DocumentStatus.Confirmed;
        }

        public void Post(DateTime postDate)
        {
            if (LifecycleStatus is not (DocumentStatus.Draft or DocumentStatus.Confirmed))
                throw new CannotModifyPostedDocumentException($"Only a Draft or Confirmed adjustment can be posted (current status: {LifecycleStatus}).");
            EnsureHasLines();

            LifecycleStatus = DocumentStatus.Posted;
            Posted = true;

            foreach (var line in _lines)
                _domainEvents.Add(new StockAdjustedDomainEvent(Id, line.ProductId, StockId, line.Direction, line.Quantity, ReasonId));
        }

        public void Cancel()
        {
            if (LifecycleStatus == DocumentStatus.Posted)
                throw new CannotCancelPostedDocumentWithoutReversalException("A posted adjustment must be reversed, not cancelled.");

            LifecycleStatus = DocumentStatus.Cancelled;
        }

        private void EnsureDraft()
        {
            if (LifecycleStatus != DocumentStatus.Draft)
                throw new CannotModifyPostedDocumentException($"Cannot modify an adjustment that is {LifecycleStatus}.");
        }

        private void EnsureHasLines()
        {
            if (_lines.Count == 0)
                throw new InventoryDocumentLineRequiredException("An adjustment must have at least one line.");
        }
    }
}
