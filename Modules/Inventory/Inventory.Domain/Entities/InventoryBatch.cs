namespace Inventory.Domain
{
    /// <summary>
    /// A lot/batch of a batch-controlled Item (brief §16). Only meaningful when
    /// Product.TrackingType includes Batch — Inventory.Application enforces that gate before ever
    /// creating one; the aggregate itself just enforces its own shape (a batch number is required
    /// and unique per item, expiry is a plain optional date).
    /// </summary>
    [Table("InventoryBatch")]
    public class InventoryBatch : BaseModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected InventoryBatch() { }

        [ForeignKey("Product")]
        public virtual long ProductId { get; private set; }

        public virtual Product? Product { get; set; }

        [Required, StringLength(100)]
        public virtual string BatchNumber { get; private set; } = string.Empty;

        public virtual DateTime? ManufacturingDate { get; private set; }

        public virtual DateTime? ExpiryDate { get; private set; }

        [StringLength(100)]
        public virtual string? SupplierBatchNumber { get; private set; }

        public virtual BatchStatus BatchStatus { get; private set; }

        public static InventoryBatch Create(long productId, string batchNumber, DateTime? manufacturingDate, DateTime? expiryDate, string? supplierBatchNumber)
        {
            if (productId <= 0)
                throw new InvalidBatchException("A batch must belong to a valid item.");
            if (string.IsNullOrWhiteSpace(batchNumber))
                throw new InvalidBatchException("A batch number is required.");
            if (expiryDate.HasValue && manufacturingDate.HasValue && expiryDate.Value.Date < manufacturingDate.Value.Date)
                throw new InvalidBatchException("A batch's expiry date cannot be before its manufacturing date.");

            var batch = new InventoryBatch
            {
                ProductId = productId,
                BatchNumber = batchNumber.Trim(),
                ManufacturingDate = manufacturingDate,
                ExpiryDate = expiryDate,
                SupplierBatchNumber = supplierBatchNumber,
                BatchStatus = BatchStatus.Active
            };

            batch._domainEvents.Add(new BatchReceivedDomainEvent(batch.Id, productId, batch.BatchNumber, expiryDate));
            return batch;
        }

        public bool IsExpired(DateTime asOfDate) => ExpiryDate.HasValue && ExpiryDate.Value.Date < asOfDate.Date;

        /// <summary>Throws unless the caller explicitly overrides — brief §16: "Expired batch cannot
        /// normally be issued." The override is a deliberate application-layer decision (e.g. a
        /// supervisor permission), never a silent domain default.</summary>
        public void EnsureIssuable(DateTime asOfDate, bool allowExpiredOverride)
        {
            if (BatchStatus == BatchStatus.Quarantine)
                throw new InvalidBatchException($"Batch {BatchNumber} is under quarantine and cannot be issued.");
            if (!allowExpiredOverride && IsExpired(asOfDate))
                throw new BatchExpiredException($"Batch {BatchNumber} expired on {ExpiryDate:d} and cannot be issued without an explicit override.");
        }

        public void Quarantine() => BatchStatus = BatchStatus.Quarantine;

        public void Activate() => BatchStatus = BatchStatus.Active;

        public void MarkDepleted() => BatchStatus = BatchStatus.Depleted;

        public void RefreshExpiryStatus(DateTime asOfDate)
        {
            if (BatchStatus == BatchStatus.Active && IsExpired(asOfDate))
                BatchStatus = BatchStatus.Expired;
        }
    }
}
