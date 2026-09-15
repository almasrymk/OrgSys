namespace Inventory.Domain
{
    /// <summary>
    /// Formalizes the transfer workflow the existing TransactionTypeId 3/4 pairing already performs
    /// (Transaction TransferIssue at the source + a linked Transaction TransferReceipt at the
    /// destination, correlated by ParentId — see docs/ddd/inventory-current-state.md §5) as an
    /// explicit aggregate with its own lifecycle, instead of that pairing being triggered ad hoc from
    /// a raw Transaction create. A transfer NEVER changes a movement's WarehouseId (brief §11) — Ship()
    /// and Receive() each raise their own domain event, and the application-layer posting handler
    /// creates two separate InventoryMovement rows (issue at FromStockId, receipt at ToStockId), same
    /// as today's TransferReceivedIntegration behavior, just behind this aggregate's Post operations
    /// instead of being auto-generated as a side effect.
    /// </summary>
    [Table("StockTransfer")]
    public class StockTransfer : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        private readonly List<StockTransferLine> _lines = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        public virtual IReadOnlyCollection<StockTransferLine> Lines => _lines.AsReadOnly();

        protected StockTransfer() { }

        [ForeignKey("FromStock")]
        public virtual long FromStockId { get; private set; }

        public virtual Stock? FromStock { get; set; }

        [ForeignKey("ToStock")]
        public virtual long ToStockId { get; private set; }

        public virtual Stock? ToStock { get; set; }

        public virtual StockTransferStatus TransferStatus { get; private set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }

        public static StockTransfer Create(long fromStockId, long toStockId, DateTime date, long createUserId, DateTime createDate, long? branchId, string? notes)
        {
            if (fromStockId <= 0 || toStockId <= 0)
                throw new InvalidStockTransferException("A transfer must specify both a source and a destination warehouse.");
            if (fromStockId == toStockId)
                throw new InvalidStockTransferException("A transfer's source and destination warehouse must be different.");

            return new StockTransfer
            {
                FromStockId = fromStockId,
                ToStockId = toStockId,
                Date = date,
                CreateUserId = createUserId,
                CreateDate = createDate,
                BranchId = branchId,
                Notes = notes,
                TransferStatus = StockTransferStatus.Draft
            };
        }

        public StockTransferLine AddLine(long productId, long unitId, decimal quantity, long? batchId, string? notes)
        {
            EnsureDraft();
            var line = new StockTransferLine(productId, unitId, quantity, batchId, notes)
            {
                StockTransfer = this,
                StockTransferId = Id,
                RowNumber = _lines.Count + 1
            };
            _lines.Add(line);
            return line;
        }

        public void RemoveLine(StockTransferLine line)
        {
            EnsureDraft();
            _lines.Remove(line);
        }

        public void Confirm()
        {
            EnsureDraft();
            EnsureHasLines();
            TransferStatus = StockTransferStatus.Confirmed;
        }

        /// <summary>Posts the issue side at FromStockId. The application-layer posting handler reacts
        /// to the raised event by creating the TransferIssue InventoryMovement and calling
        /// InventoryBalance.IssueOut on the source balance.</summary>
        public void Ship(DateTime shipDate)
        {
            if (TransferStatus is not (StockTransferStatus.Draft or StockTransferStatus.Confirmed))
                throw new TransferAlreadyPostedException($"Only a Draft or Confirmed transfer can be shipped (current status: {TransferStatus}).");
            EnsureHasLines();

            TransferStatus = StockTransferStatus.Shipped;
            _domainEvents.Add(new StockTransferShippedDomainEvent(Id, FromStockId, ToStockId));
        }

        /// <summary>Posts the receipt side at ToStockId and completes the transfer. Immediate
        /// (non-transit) transfers call Ship() then Receive() back-to-back in the same request,
        /// matching today's "AutoReceived" preference behavior; a transit workflow calls Receive()
        /// later, once the goods actually arrive.</summary>
        public void Receive(DateTime receiveDate)
        {
            if (TransferStatus != StockTransferStatus.Shipped)
                throw new TransferAlreadyPostedException($"Only a Shipped transfer can be received (current status: {TransferStatus}).");

            TransferStatus = StockTransferStatus.Completed;
            Posted = true;
            _domainEvents.Add(new StockTransferReceivedDomainEvent(Id, FromStockId, ToStockId));
        }

        public void Cancel()
        {
            if (TransferStatus is StockTransferStatus.Shipped or StockTransferStatus.Completed)
                throw new CannotCancelPostedDocumentWithoutReversalException("A shipped or completed transfer must be reversed, not cancelled.");

            TransferStatus = StockTransferStatus.Cancelled;
        }

        private void EnsureDraft()
        {
            if (TransferStatus != StockTransferStatus.Draft)
                throw new CannotModifyPostedDocumentException($"Cannot modify a transfer that is {TransferStatus}.");
        }

        private void EnsureHasLines()
        {
            if (_lines.Count == 0)
                throw new InventoryDocumentLineRequiredException("A transfer must have at least one line.");
        }
    }
}
