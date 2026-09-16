namespace Inventory.Domain
{
    /// <summary>
    /// One physically serialized unit of a serial-controlled Item (brief §17). Quantity is always
    /// implicitly 1 — there is no Quantity property, by design, so "1 per serial" cannot be violated.
    /// </summary>
    [Table("InventorySerial")]
    public class InventorySerial : BaseModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected InventorySerial() { }

        /// <summary>Scalar-only Catalog Product reference. FK preserved in OrgContext.</summary>
        public virtual long ProductId { get; private set; }

        [Required, StringLength(100)]
        public virtual string SerialNumber { get; private set; } = string.Empty;

        [ForeignKey("Batch")]
        public virtual long? BatchId { get; private set; }

        public virtual InventoryBatch? Batch { get; set; }

        [ForeignKey("CurrentStock")]
        public virtual long? CurrentStockId { get; private set; }

        public virtual Stock? CurrentStock { get; set; }

        [ForeignKey("CurrentLocation")]
        public virtual long? CurrentLocationId { get; private set; }

        public virtual WarehouseLocation? CurrentLocation { get; set; }

        public virtual SerialStatus SerialStatus { get; private set; }

        public virtual DateTime ReceivedDate { get; private set; }

        public virtual DateTime? IssuedDate { get; private set; }

        public static InventorySerial Receive(long productId, string serialNumber, long stockId, long? locationId, long? batchId, DateTime receivedDate)
        {
            if (productId <= 0)
                throw new SerialNotAvailableException("A serial must belong to a valid item.");
            if (string.IsNullOrWhiteSpace(serialNumber))
                throw new SerialNotAvailableException("A serial number is required.");

            var serial = new InventorySerial
            {
                ProductId = productId,
                SerialNumber = serialNumber.Trim(),
                CurrentStockId = stockId,
                CurrentLocationId = locationId,
                BatchId = batchId,
                SerialStatus = SerialStatus.Available,
                ReceivedDate = receivedDate
            };

            serial._domainEvents.Add(new SerialReceivedDomainEvent(serial.Id, productId, serial.SerialNumber, stockId));
            return serial;
        }

        public void Reserve()
        {
            if (SerialStatus != SerialStatus.Available)
                throw new SerialNotAvailableException($"Serial {SerialNumber} is {SerialStatus} and cannot be reserved.");
            SerialStatus = SerialStatus.Reserved;
        }

        public void ReleaseReservation()
        {
            if (SerialStatus != SerialStatus.Reserved)
                throw new SerialNotAvailableException($"Serial {SerialNumber} is not currently reserved.");
            SerialStatus = SerialStatus.Available;
        }

        /// <summary>Guards brief §17's core invariant: a serial cannot be issued twice. Only
        /// Available or Reserved (by this same fulfillment) can transition to Issued.</summary>
        public void MarkIssued(DateTime issueDate)
        {
            if (SerialStatus is not (SerialStatus.Available or SerialStatus.Reserved))
                throw new SerialAlreadyIssuedException($"Serial {SerialNumber} is already {SerialStatus} and cannot be issued again.");

            SerialStatus = SerialStatus.Issued;
            IssuedDate = issueDate;
            CurrentStockId = null;
            CurrentLocationId = null;
        }

        public void MarkReturned(long stockId, long? locationId)
        {
            if (SerialStatus != SerialStatus.Issued)
                throw new SerialNotAvailableException($"Serial {SerialNumber} is not currently issued.");

            SerialStatus = SerialStatus.Returned;
            CurrentStockId = stockId;
            CurrentLocationId = locationId;
            IssuedDate = null;
        }

        /// <summary>Moves the serial to a new warehouse/location — used by StockTransfer posting.
        /// A serial can only ever be in one place, so this simply overwrites its current location
        /// rather than requiring a two-sided balance update (brief §17: "cannot simultaneously exist
        /// in two warehouse locations" — there is exactly one CurrentStockId/CurrentLocationId at
        /// any time by construction).</summary>
        public void MoveTo(long stockId, long? locationId)
        {
            if (SerialStatus is not (SerialStatus.Available or SerialStatus.Reserved))
                throw new SerialNotAvailableException($"Serial {SerialNumber} is {SerialStatus} and cannot be transferred.");

            CurrentStockId = stockId;
            CurrentLocationId = locationId;
        }

        public void MarkDamaged() => SerialStatus = SerialStatus.Damaged;

        public void MarkLost() => SerialStatus = SerialStatus.Lost;

        public void Quarantine() => SerialStatus = SerialStatus.Quarantine;
    }
}
