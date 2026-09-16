namespace Inventory.Domain
{
    /// <summary>
    /// A standalone inventory issue (brief §10) — independent of Sales' own document model. Mirrors
    /// InventoryReceipt's lifecycle. Availability (Available ≥ Requested, unless the warehouse/item
    /// allows negative stock) is NOT checked here — that requires reading InventoryBalance, which is
    /// outside this aggregate's boundary; the application-layer posting handler performs that check
    /// via InventoryBalance.IssueOut before treating the resulting StockIssuedDomainEvent as final.
    /// </summary>
    [Table("InventoryIssue")]
    public class InventoryIssue : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        private readonly List<InventoryIssueLine> _lines = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        public virtual IReadOnlyCollection<InventoryIssueLine> Lines => _lines.AsReadOnly();

        protected InventoryIssue() { }

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

        public virtual SourceDocumentType? SourceType { get; private set; }

        public virtual long? SourceId { get; private set; }

        public static InventoryIssue Create(
            long stockId, long? locationId, long? dealerId, DateTime date,
            long createUserId, DateTime createDate, long? branchId, string? notes,
            SourceDocumentType? sourceType = null, long? sourceId = null)
        {
            if (stockId <= 0)
                throw new InventoryDocumentLineRequiredException("An issue must target a valid warehouse.");

            return new InventoryIssue
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
                SourceId = sourceId
            };
        }

        public InventoryIssueLine AddLine(long productId, long unitId, decimal quantity, long? batchId, long? serialId, string? notes)
        {
            EnsureDraft();
            var line = new InventoryIssueLine(productId, unitId, quantity, batchId, serialId, notes)
            {
                InventoryIssue = this,
                InventoryIssueId = Id,
                RowNumber = _lines.Count + 1
            };
            _lines.Add(line);
            return line;
        }

        public void RemoveLine(InventoryIssueLine line)
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
                throw new CannotModifyPostedDocumentException($"Only a Draft or Confirmed issue can be posted (current status: {LifecycleStatus}).");
            EnsureHasLines();

            LifecycleStatus = DocumentStatus.Posted;
            Posted = true;

            foreach (var line in _lines)
                _domainEvents.Add(new StockIssuedDomainEvent(Id, line.ProductId, StockId, LocationId, line.Quantity, line.BatchId, line.SerialId));
        }

        public void Cancel()
        {
            if (LifecycleStatus == DocumentStatus.Posted)
                throw new CannotCancelPostedDocumentWithoutReversalException("A posted issue must be reversed, not cancelled.");

            LifecycleStatus = DocumentStatus.Cancelled;
        }

        private void EnsureDraft()
        {
            if (LifecycleStatus != DocumentStatus.Draft)
                throw new CannotModifyPostedDocumentException($"Cannot modify an issue that is {LifecycleStatus}.");
        }

        private void EnsureHasLines()
        {
            if (_lines.Count == 0)
                throw new InventoryDocumentLineRequiredException("An issue must have at least one line.");
        }
    }
}
