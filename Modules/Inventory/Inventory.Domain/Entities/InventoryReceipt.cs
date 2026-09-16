namespace Inventory.Domain
{
    /// <summary>
    /// A standalone inventory receipt (brief §9) — independent of Purchasing's own document model.
    /// Draft while lines can be edited; Posted lines are immutable and Posting emits one
    /// StockReceivedDomainEvent per line, which the application-layer posting service turns into the
    /// canonical InventoryMovement (Transaction/TransactionProduct, MovementType.Receipt) rows plus
    /// an InventoryBalance.Receive() call — this aggregate never touches the ledger/balance itself
    /// (brief §34: keep aggregate boundaries small, don't load movements into a document aggregate).
    /// </summary>
    [Table("InventoryReceipt")]
    public class InventoryReceipt : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        private readonly List<InventoryReceiptLine> _lines = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        public virtual IReadOnlyCollection<InventoryReceiptLine> Lines => _lines.AsReadOnly();

        protected InventoryReceipt() { }

        [ForeignKey("Stock")]
        public virtual long StockId { get; private set; }

        public virtual Stock? Stock { get; set; }

        [ForeignKey("Location")]
        public virtual long? LocationId { get; private set; }

        public virtual WarehouseLocation? Location { get; set; }

        /// <summary>Scalar-only reference into Parties.Domain.Dealer — no EF navigation.
        /// FK preserved via Fluent HasOne(typeof(Dealer)) in OrgContext.</summary>
        public virtual long? DealerId { get; private set; }

        public virtual DocumentStatus LifecycleStatus { get; private set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }

        /// <summary>Idempotency correlation to the originating document (brief §31) — e.g. the
        /// CommercialDocuments.Invoice that triggered this receipt. Null for a manually-created
        /// receipt.</summary>
        public virtual SourceDocumentType? SourceType { get; private set; }

        public virtual long? SourceId { get; private set; }

        /// <summary>Scalar-only Purchasing.PurchaseOrder link. No Domain navigation.
        /// FK via Fluent HasOne(typeof(PurchaseOrder)) in OrgContext.</summary>
        public virtual long? PurchaseOrderId { get; private set; }

        public static InventoryReceipt Create(
            long stockId, long? locationId, long? dealerId, DateTime date,
            long createUserId, DateTime createDate, long? branchId, string? notes,
            SourceDocumentType? sourceType = null, long? sourceId = null,
            long? purchaseOrderId = null)
        {
            if (stockId <= 0)
                throw new InventoryDocumentLineRequiredException("A receipt must target a valid warehouse.");

            return new InventoryReceipt
            {
                StockId = stockId,
                LocationId = locationId,
                DealerId = dealerId,
                Date = date,
                CreateUserId = createUserId,
                CreateDate = createDate,
                BranchId = branchId,
                Notes = notes,
                LifecycleStatus = DocumentStatus.Draft,
                SourceType = sourceType,
                SourceId = sourceId,
                PurchaseOrderId = purchaseOrderId
            };
        }

        public InventoryReceiptLine AddLine(long productId, long unitId, decimal quantity, decimal unitCost, long? batchId, string? notes)
        {
            EnsureDraft();
            var line = new InventoryReceiptLine(productId, unitId, quantity, unitCost, batchId, notes)
            {
                InventoryReceipt = this,
                InventoryReceiptId = Id,
                RowNumber = _lines.Count + 1
            };
            _lines.Add(line);
            return line;
        }

        public void RemoveLine(InventoryReceiptLine line)
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

        /// <summary>Draft or Confirmed -> Posted. Raises one StockReceivedDomainEvent per line —
        /// the application-layer posting handler is responsible for actually creating the
        /// InventoryMovement rows and updating InventoryBalance from these events, inside the same
        /// transaction (brief §32).</summary>
        public void Post(DateTime postDate)
        {
            if (LifecycleStatus is not (DocumentStatus.Draft or DocumentStatus.Confirmed))
                throw new CannotModifyPostedDocumentException($"Only a Draft or Confirmed receipt can be posted (current status: {LifecycleStatus}).");
            EnsureHasLines();

            LifecycleStatus = DocumentStatus.Posted;
            Posted = true;

            foreach (var line in _lines)
                _domainEvents.Add(new StockReceivedDomainEvent(Id, line.ProductId, StockId, LocationId, line.Quantity, line.UnitCost, line.BatchId));
        }

        public void Cancel()
        {
            if (LifecycleStatus == DocumentStatus.Posted)
                throw new CannotCancelPostedDocumentWithoutReversalException("A posted receipt must be reversed, not cancelled.");

            LifecycleStatus = DocumentStatus.Cancelled;
        }

        private void EnsureDraft()
        {
            if (LifecycleStatus != DocumentStatus.Draft)
                throw new CannotModifyPostedDocumentException($"Cannot modify a receipt that is {LifecycleStatus}.");
        }

        private void EnsureHasLines()
        {
            if (_lines.Count == 0)
                throw new InventoryDocumentLineRequiredException("A receipt must have at least one line.");
        }
    }
}
