namespace Purchasing.Domain
{
    /// <summary>
    /// A confirmed order to a specific supplier — the Purchasing-owned counterpart to Sales'
    /// SalesOrder. Lifecycle (unchanged from the pre-hardening shape, reuses the shared
    /// OrgSys.SharedKernel.Status enum exactly as before — no new column): Status.New until
    /// LinkInvoice() is called once the resulting Purchase Invoice
    /// (CommercialDocuments.Domain.Invoice, InvoiceType=Purchase) has been created the normal way
    /// and linked here, at which point Status becomes Approved. No automatic Invoice generation
    /// happens anywhere in this module (brief §40) — Purchasing never owns the financial invoice.
    /// DealerId/Dealer keep their existing EF navigation to Parties.Domain.Dealer — an
    /// already-accepted architecture exception, same convention every other module's Dealer
    /// reference uses; not something this hardening pass removes. Cancel and the
    /// RecordReceipt/RecordReturn/CancelLine quantity-tracking methods are new, additive capability
    /// (brief §26/§33/§43) that did not exist as reachable operations previously — actual Inventory
    /// stock-movement integration remains a separate, later phase (brief §37), mirroring how
    /// Sales.Domain.SalesOrder defers its own Inventory/AR integration. Mirrors
    /// Sales.Domain.SalesOrder's aggregate shape throughout.
    /// </summary>
    [Table("PurchaseOrder")]
    public class PurchaseOrder : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        private readonly List<PurchaseOrderProduct> _lines = [];

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        public virtual IReadOnlyCollection<PurchaseOrderProduct> PurchaseOrderProducts => _lines.AsReadOnly();

        /// <summary>EF materialization constructor only. Business code creates a PurchaseOrder
        /// through <see cref="Create"/>.</summary>
        protected PurchaseOrder() { }

        [ForeignKey("Dealer")]
        public virtual long DealerId { get; private set; }

        public virtual Dealer? Dealer { get; set; }

        /// <summary>Optional provenance link back to the requisition this order was raised from.</summary>
        public virtual long? PurchaseRequisitionId { get; private set; }

        public virtual PurchaseRequisition? PurchaseRequisition { get; set; }

        /// <summary>Set once a Purchase Invoice has been created and linked — see LinkInvoice. No EF
        /// navigation to CommercialDocuments.Domain.Invoice: resolved via CommercialDocuments.Contracts
        /// (GetInvoiceReferenceQuery) at the Application layer, same boundary discipline as
        /// PurchaseOrderProduct.ProductId.</summary>
        public virtual long? InvoiceId { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Discount { get; private set; }

        public virtual int DiscountType { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Tax { get; private set; }

        public virtual int TaxType { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Net { get; private set; }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }

        // ----- Construction -----

        public static PurchaseOrder Create(
            long dealerId,
            long createUserId,
            DateTime createDate,
            long? purchaseRequisitionId = null,
            long? branchId = null,
            decimal discount = 0,
            int discountType = 0,
            decimal tax = 0,
            int taxType = 0,
            string? notes = null)
        {
            if (dealerId <= 0)
                throw new InvalidPurchaseOrderException("A purchase order's supplier is required.");
            if (discount < 0 || tax < 0)
                throw new InvalidPurchaseOrderException("A purchase order's discount/tax cannot be negative.");

            var order = new PurchaseOrder
            {
                DealerId = dealerId,
                PurchaseRequisitionId = purchaseRequisitionId,
                Date = createDate,
                CreateUserId = createUserId,
                CreateDate = createDate,
                BranchId = branchId,
                Discount = discount,
                DiscountType = discountType,
                Tax = tax,
                TaxType = taxType,
                Notes = notes,
                Status = Status.New
            };
            order.RecalculateTotals();

            order.Raise(new PurchaseOrderCreatedDomainEvent(order.Id, dealerId));
            return order;
        }

        // ----- Line mutation (New/draft only) -----

        public PurchaseOrderProduct AddLine(long productId, long unitId, decimal quantity, decimal price, string? notes = null)
        {
            EnsureEditable();

            if (quantity <= 0)
                throw new InvalidPurchaseOrderLineException("A purchase order line's quantity must be greater than zero.");
            if (price < 0)
                throw new InvalidPurchaseOrderLineException("A purchase order line's price cannot be negative.");

            var line = new PurchaseOrderProduct(_lines.Count + 1, productId, unitId, quantity, price, notes)
            {
                PurchaseOrderId = Id
            };
            _lines.Add(line);
            RecalculateTotals();
            return line;
        }

        public void UpdateLine(long lineId, long productId, long unitId, decimal quantity, decimal price, string? notes = null)
        {
            EnsureEditable();

            if (quantity <= 0)
                throw new InvalidPurchaseOrderLineException("A purchase order line's quantity must be greater than zero.");
            if (price < 0)
                throw new InvalidPurchaseOrderLineException("A purchase order line's price cannot be negative.");

            var line = _lines.FirstOrDefault(l => l.Id == lineId)
                ?? throw new InvalidPurchaseOrderLineException($"Line {lineId} does not belong to this purchase order.");

            line.UpdateCommercial(productId, unitId, quantity, price, notes);
            RecalculateTotals();
        }

        public void RemoveLine(long lineId)
        {
            EnsureEditable();

            var line = _lines.FirstOrDefault(l => l.Id == lineId);
            if (line is not null)
            {
                _lines.Remove(line);
                RecalculateTotals();
            }
        }

        public void UpdateHeader(decimal discount, int discountType, decimal tax, int taxType, string? notes)
        {
            EnsureEditable();

            if (discount < 0 || tax < 0)
                throw new InvalidPurchaseOrderException("A purchase order's discount/tax cannot be negative.");

            Discount = discount;
            DiscountType = discountType;
            Tax = tax;
            TaxType = taxType;
            Notes = notes;
            RecalculateTotals();
        }

        // ----- Lifecycle -----

        /// <summary>
        /// Records that a Purchase Invoice has been created (the normal way, via
        /// CommercialDocuments' own Invoice endpoints) and links it here — mirrors the pre-hardening
        /// LinkInvoiceCommandHandler exactly. Purchasing never creates the invoice itself (brief §40).
        /// Guarding on InvoiceId being unset is what makes a duplicate/retried link request fail
        /// rather than silently relink (brief §67 idempotency).
        /// </summary>
        public void LinkInvoice(long invoiceId)
        {
            if (InvoiceId is > 0)
                throw new PurchaseOrderAlreadyLinkedException("This purchase order is already linked to an invoice.");
            if (invoiceId <= 0)
                throw new InvalidPurchaseOrderException("A valid invoice id is required to link a purchase order.");

            InvoiceId = invoiceId;
            Status = Status.Approved;
            Raise(new PurchaseOrderLinkedToInvoiceDomainEvent(Id, invoiceId));
        }

        /// <summary>New -> Cancel. Never valid once anything has been received or linked to an
        /// invoice — use RecordReceipt/CancelLine to wind down an in-progress order instead. Mirrors
        /// Payables.Domain.Payable.Cancel's "only if untouched" rule. Checking ReceivedQuantity
        /// directly (not just Status) matters because LinkInvoice is the only thing that moves
        /// Status off New — RecordReceipt deliberately does not (see EnsureOpenForFulfillment's
        /// remark), so an order with partial receipt history but no linked invoice is still
        /// Status.New and would otherwise slip past a Status-only guard.</summary>
        public void Cancel()
        {
            if (Status == Status.Cancel)
                return;
            if (Status != Status.New)
                throw new PurchaseOrderCannotBeCancelledException($"Cannot cancel a purchase order that is {Status} — an order with a linked invoice must be wound down via reversal instead.");
            if (_lines.Any(l => l.ReceivedQuantity > 0))
                throw new PurchaseOrderCannotBeCancelledException("Cannot cancel a purchase order that already has receipt history — use CancelLine to wind down the remaining quantity instead.");

            Status = Status.Cancel;
            Raise(new PurchaseOrderCancelledDomainEvent(Id));
        }

        /// <summary>
        /// Records that <paramref name="quantity"/> of <paramref name="lineId"/> was physically
        /// received — called by Purchasing.Application AFTER Inventory (a future phase) has
        /// confirmed the actual stock receipt; this method never moves stock itself (brief §37).
        /// Rejects receiving beyond what remains on the line (brief §33/§34 over-receipt prevention)
        /// and on a Cancelled/Rejected order.
        /// </summary>
        public void RecordReceipt(long lineId, decimal quantity)
        {
            EnsureOpenForFulfillment();

            if (quantity <= 0)
                throw new InvalidPurchaseOrderReceiptException("A received quantity must be greater than zero.");

            var line = FindLine(lineId, "received");
            line.RecordReceipt(quantity);

            Raise(_lines.Sum(l => l.RemainingQuantity) == 0
                ? new PurchaseOrderCompletedDomainEvent(Id)
                : new PurchaseOrderPartiallyReceivedDomainEvent(Id));
        }

        /// <summary>
        /// Records that <paramref name="quantity"/> of <paramref name="lineId"/> was returned to the
        /// supplier — called AFTER the owning PurchaseReturn (a future phase) has confirmed the
        /// physical return; never issues stock itself (brief §48). Cumulative returns on a line
        /// cannot exceed what was actually received minus what was already returned (brief §46/§47),
        /// and rejects returning an item that was never received in the first place.
        /// </summary>
        public void RecordReturn(long lineId, decimal quantity)
        {
            if (quantity <= 0)
                throw new InvalidPurchaseOrderReturnException("A returned quantity must be greater than zero.");

            var line = FindLine(lineId, "returned");
            line.RecordReturn(quantity);
        }

        /// <summary>Cancels the remaining (unreceived) quantity of a single line — a partial
        /// cancellation. Never reduces Quantity itself; received history is untouched.</summary>
        public void CancelLine(long lineId, decimal quantity)
        {
            EnsureOpenForFulfillment();

            if (quantity <= 0)
                throw new InvalidPurchaseOrderLineException("A line cancellation quantity must be greater than zero.");

            var line = FindLine(lineId, "cancelled");
            if (quantity > line.RemainingQuantity)
                throw new InvalidPurchaseOrderLineException($"Cannot cancel {quantity} on line for product {line.ProductId} — only {line.RemainingQuantity} remains.");

            line.CancelPartial(quantity);

            if (_lines.Sum(l => l.RemainingQuantity) == 0)
                Raise(new PurchaseOrderCompletedDomainEvent(Id));
        }

        private void RecalculateTotals()
        {
            Total = _lines.Sum(l => l.Total);
            Net = Total - Discount + Tax;
        }

        private PurchaseOrderProduct FindLine(long lineId, string action) =>
            _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidPurchaseOrderLineException($"Line {lineId} does not belong to this purchase order and cannot be {action}.");

        private void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        private void EnsureEditable()
        {
            if (Status != Status.New)
                throw new PurchaseOrderNotEditableException("Only a New (draft) purchase order can be edited.");
        }

        private void EnsureOpenForFulfillment()
        {
            if (Status is Status.Cancel or Status.Rejected or Status.Deleted)
                throw new PurchaseOrderNotOpenException($"Cannot record a receipt/cancellation against a purchase order that is {Status}.");
        }
    }
}
