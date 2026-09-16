namespace Inventory.Domain
{
    /// <summary>
    /// Reserves Available stock for a source document (typically a Sales Order) without ever
    /// touching OnHand (brief §14 — the critical rule: "Reservation does NOT change physical
    /// stock"). The actual OnHand/Reserved bookkeeping lives on InventoryBalance
    /// (Reserve/ReleaseReservation/FulfillReservation) — this aggregate is the reservation's own
    /// identity/audit record: who asked for what, why, and its own lifecycle, independent of the
    /// balance row it affected (a balance row is shared by many reservations over time).
    /// </summary>
    [Table("StockReservation")]
    public class StockReservation : BaseModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected StockReservation() { }

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

        [ForeignKey("Serial")]
        public virtual long? SerialId { get; private set; }

        public virtual InventorySerial? Serial { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public virtual decimal Quantity { get; private set; }

        public virtual SourceDocumentType SourceType { get; private set; }

        public virtual long SourceId { get; private set; }

        public virtual long? SourceLineId { get; private set; }

        public virtual ReservationStatus ReservationStatus { get; private set; }

        public virtual DateTime? ExpiresAt { get; private set; }

        public virtual long CreateUserId { get; private set; }

        public virtual DateTime CreateDate { get; private set; }

        public virtual DateTime? ReleasedAt { get; private set; }

        public virtual DateTime? FulfilledAt { get; private set; }

        /// <summary>
        /// Creates the reservation record. This does NOT itself reserve against InventoryBalance —
        /// the caller (ReserveStockCommandHandler) reserves against the balance first (inside the
        /// same concurrency-safe retry loop, docs/ddd/inventory-target-architecture.md §8) and only
        /// calls Create once that succeeds, so a StockReservation row never exists without a matching
        /// InventoryBalance.QuantityReserved increment.
        /// </summary>
        public static StockReservation Create(
            long productId, long stockId, long? locationId, long? batchId, long? serialId,
            decimal quantity, SourceDocumentType sourceType, long sourceId, long? sourceLineId,
            long createUserId, DateTime createDate, DateTime? expiresAt)
        {
            if (quantity <= 0)
                throw new InsufficientStockException("A reservation quantity must be greater than zero.");
            if (sourceId <= 0)
                throw new ReservationNotActiveException("A reservation must reference a valid source document.");

            var reservation = new StockReservation
            {
                ProductId = productId,
                StockId = stockId,
                LocationId = locationId,
                BatchId = batchId,
                SerialId = serialId,
                Quantity = quantity,
                SourceType = sourceType,
                SourceId = sourceId,
                SourceLineId = sourceLineId,
                ReservationStatus = ReservationStatus.Active,
                CreateUserId = createUserId,
                CreateDate = createDate,
                ExpiresAt = expiresAt
            };

            reservation._domainEvents.Add(new StockReservedDomainEvent(productId, stockId, locationId, quantity, sourceType, sourceId));
            return reservation;
        }

        /// <summary>Cancellation/return path — the caller must have already called
        /// InventoryBalance.ReleaseReservation for the same quantity.</summary>
        public void Release(DateTime releasedAt)
        {
            EnsureActive();
            ReservationStatus = ReservationStatus.Released;
            ReleasedAt = releasedAt;
            _domainEvents.Add(new StockReservationReleasedDomainEvent(ProductId, StockId, Quantity));
        }

        /// <summary>Delivery/issue path — the caller must have already called
        /// InventoryBalance.FulfillReservation for the same quantity.</summary>
        public void Fulfill(DateTime fulfilledAt)
        {
            EnsureActive();
            ReservationStatus = ReservationStatus.Fulfilled;
            FulfilledAt = fulfilledAt;
            _domainEvents.Add(new StockReservationFulfilledDomainEvent(ProductId, StockId, Quantity));
        }

        public void Expire(DateTime expiredAt)
        {
            EnsureActive();
            ReservationStatus = ReservationStatus.Expired;
            ReleasedAt = expiredAt;
            _domainEvents.Add(new StockReservationReleasedDomainEvent(ProductId, StockId, Quantity));
        }

        public void Cancel(DateTime cancelledAt)
        {
            EnsureActive();
            ReservationStatus = ReservationStatus.Cancelled;
            ReleasedAt = cancelledAt;
            _domainEvents.Add(new StockReservationReleasedDomainEvent(ProductId, StockId, Quantity));
        }

        public bool IsExpired(DateTime asOfDate) =>
            ReservationStatus == ReservationStatus.Active && ExpiresAt.HasValue && ExpiresAt.Value <= asOfDate;

        private void EnsureActive()
        {
            if (ReservationStatus != ReservationStatus.Active)
                throw new ReservationAlreadyFulfilledException($"Reservation {Id} is already {ReservationStatus}.");
        }
    }
}
