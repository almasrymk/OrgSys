namespace Purchasing.Domain
{
    /// <summary>
    /// An internal request to buy something, raised before a supplier/price is chosen. Lifecycle
    /// (unchanged from the pre-hardening shape, reuses the shared OrgSys.SharedKernel.Status enum
    /// exactly as before — no new column): Status.New (draft, still editable) → Status.UnderReview
    /// (submitted) → Status.Approved (sourced into a PurchaseOrder — see RecordSourced). No separate
    /// approval gate is modeled (submitting does not require anyone else's sign-off) — same as
    /// before hardening. Reject/Cancel are new, additive capabilities (brief §11/§61) that did not
    /// exist as reachable operations previously. Mirrors Advances.Domain.Custody/Sales.Domain.Quotation's
    /// aggregate shape (private setters, protected EF ctor, static Create factory, manual
    /// domain-event list because MovementModel can't also inherit OrgSys.SharedKernel.AggregateRoot).
    /// </summary>
    [Table("PurchaseRequisition")]
    public class PurchaseRequisition : MovementModel
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        private readonly List<PurchaseRequisitionProduct> _lines = [];

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        public virtual IReadOnlyCollection<PurchaseRequisitionProduct> PurchaseRequisitionProducts => _lines.AsReadOnly();

        /// <summary>EF materialization constructor only. Business code creates a PurchaseRequisition
        /// through <see cref="Create"/>.</summary>
        protected PurchaseRequisition() { }

        [StringLength(500)]
        public virtual string? Notes { get; private set; }

        // ----- Construction -----

        public static PurchaseRequisition Create(long createUserId, DateTime createDate, long? branchId = null, string? notes = null)
        {
            var requisition = new PurchaseRequisition
            {
                CreateUserId = createUserId,
                CreateDate = createDate,
                Date = createDate,
                BranchId = branchId,
                Notes = notes,
                Status = Status.New
            };

            requisition.Raise(new PurchaseRequisitionCreatedDomainEvent(requisition.Id));
            return requisition;
        }

        // ----- Line mutation (New/draft only) -----

        public PurchaseRequisitionProduct AddLine(long productId, long unitId, decimal quantity, string? notes = null)
        {
            EnsureEditable();

            if (quantity <= 0)
                throw new InvalidPurchaseRequisitionLineException("A purchase requisition line's quantity must be greater than zero.");

            var line = new PurchaseRequisitionProduct(_lines.Count + 1, productId, unitId, quantity, notes)
            {
                PurchaseRequisitionId = Id
            };
            _lines.Add(line);
            return line;
        }

        public void RemoveLine(long lineId)
        {
            EnsureEditable();

            var line = _lines.FirstOrDefault(l => l.Id == lineId);
            if (line is not null)
                _lines.Remove(line);
        }

        public void UpdateNotes(string? notes)
        {
            EnsureEditable();
            Notes = notes;
        }

        // ----- Lifecycle -----

        /// <summary>New -> UnderReview. Requires at least one line.</summary>
        public void Submit()
        {
            if (Status != Status.New)
                throw new PurchaseRequisitionNotSubmittableException("Only a New (draft) purchase requisition can be submitted.");
            if (_lines.Count == 0)
                throw new PurchaseRequisitionNotSubmittableException("Cannot submit a purchase requisition with no lines.");

            Status = Status.UnderReview;
            Raise(new PurchaseRequisitionSubmittedDomainEvent(Id));
        }

        /// <summary>UnderReview -> Rejected.</summary>
        public void Reject()
        {
            if (Status != Status.UnderReview)
                throw new PurchaseRequisitionNotOpenException("Only an UnderReview purchase requisition can be rejected.");

            Status = Status.Rejected;
            Raise(new PurchaseRequisitionRejectedDomainEvent(Id));
        }

        /// <summary>New or UnderReview -> Cancel. Never valid once Approved (already sourced) — a
        /// sourced requisition's history must be preserved, not cancelled out from under its
        /// resulting PurchaseOrder(s). Idempotent if already Cancelled.</summary>
        public void Cancel()
        {
            if (Status == Status.Cancel)
                return;
            if (Status is not (Status.New or Status.UnderReview))
                throw new PurchaseRequisitionCannotBeCancelledException($"Cannot cancel a purchase requisition that is {Status}.");

            Status = Status.Cancel;
            Raise(new PurchaseRequisitionCancelledDomainEvent(Id));
        }

        /// <summary>
        /// UnderReview -> Approved. Called by Purchasing.Application (ConvertToPurchaseOrderCommand)
        /// AFTER it has created the resulting PurchaseOrder — this method only records which lines
        /// were sourced and how much, it never creates a PurchaseOrder itself (brief §12: conversion
        /// creates a SEPARATE document; the requisition stays historical). Today's caller always
        /// passes every line at its full remaining quantity (whole-requisition conversion, matching
        /// the pre-hardening behavior exactly), but accepting a per-line quantity map here — rather
        /// than always consuming every line in full — lays the groundwork for partial sourcing
        /// (brief §13) once RFQ exists, without changing today's behavior. Guarding on
        /// Status == UnderReview (not just "not yet Approved") is what makes a duplicate/retried
        /// conversion request fail rather than double-source (brief §67 idempotency, "cannot convert twice").
        /// </summary>
        public void RecordSourced(IReadOnlyDictionary<long, decimal> orderedQuantitiesByLineId)
        {
            if (Status != Status.UnderReview)
                throw new PurchaseRequisitionCannotBeSourcedException("Only an UnderReview purchase requisition can be sourced into a purchase order.");

            foreach (var (lineId, quantity) in orderedQuantitiesByLineId)
            {
                var line = _lines.FirstOrDefault(l => l.Id == lineId)
                    ?? throw new InvalidPurchaseRequisitionLineException($"Line {lineId} does not belong to this purchase requisition.");
                line.RecordOrdered(quantity);
            }

            Status = Status.Approved;
            Raise(new PurchaseRequisitionApprovedDomainEvent(Id));
        }

        private void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        private void EnsureEditable()
        {
            if (Status != Status.New)
                throw new PurchaseRequisitionNotEditableException("Only a New (draft) purchase requisition can be edited.");
        }
    }
}
