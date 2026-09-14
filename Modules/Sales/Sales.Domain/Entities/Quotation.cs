namespace Sales.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// A Quotation — a commercial offer to a customer, not yet a commitment (brief §9). CustomerId is
/// an opaque reference to Parties.Domain.Dealer (brief §7: Sales must not duplicate or navigate to
/// Customer) — the field is named CustomerId rather than DealerId because, in Sales' own ubiquitous
/// language, it is specifically a customer, the same naming choice
/// Receivables.Domain.Receivable.CustomerId already makes for the identical underlying Dealer.
/// Uses the inherited BaseModel.Code/CodeNumber for document numbering rather than a redundant
/// QuotationNumber field, and MovementModel.Date as the quotation's own date — the same convention
/// every other aggregate in this codebase (Financial, Journal, Payable, Custody) already follows.
/// Mirrors Advances.Domain.Custody's shape throughout (private setters, protected EF ctor, static
/// Create factory, manual domain-event list because MovementModel can't also inherit
/// OrgSys.SharedKernel.AggregateRoot).
/// </summary>
[Table("SalesQuotation")]
public class Quotation : MovementModel
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private readonly List<QuotationLine> _lines = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public virtual IReadOnlyCollection<QuotationLine> Lines => _lines.AsReadOnly();

    /// <summary>EF materialization constructor only. Business code creates a Quotation through
    /// <see cref="Create"/> — never via a bare object initializer, which would leave
    /// Subtotal/TotalAmount/LifecycleStatus in an unvalidated state.</summary>
    protected Quotation() { }

    public virtual long CustomerId { get; private set; }

    public virtual DateTime ValidUntil { get; private set; }

    public virtual long CurrencyId { get; private set; }

    [Column(TypeName = "decimal(18,6)")]
    public virtual decimal Rate { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Subtotal { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal DiscountAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TaxAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TotalAmount { get; private set; }

    public virtual QuotationStatus LifecycleStatus { get; private set; }

    /// <summary>Set only once Accepted and converted — brief §12: conversion creates a SEPARATE
    /// SalesOrder, this Quotation is never mutated into one. Reference id only, no navigation.</summary>
    public virtual long? ConvertedToSalesOrderId { get; private set; }

    [StringLength(500)]
    public virtual string? Notes { get; private set; }

    /// <summary>Derived, never stored — mirrors Advances.Domain.Custody.IsOverdue's reasoning
    /// (changes purely with the passage of time). True once ValidUntil has passed and the
    /// quotation is still open (Sent), regardless of whether Expire() has actually been called yet.</summary>
    public bool IsPastValidUntil(DateTime asOfDate) => LifecycleStatus == QuotationStatus.Sent && ValidUntil.Date < asOfDate.Date;

    // ----- Construction -----

    public static Quotation Create(
        long customerId,
        long currencyId,
        decimal rate,
        DateTime quotationDate,
        DateTime validUntil,
        long createUserId,
        DateTime createDate,
        long? branchId = null,
        string? notes = null)
    {
        if (rate <= 0)
            throw new InvalidQuotationLineException("A quotation's exchange rate must be greater than zero.");
        if (validUntil.Date < quotationDate.Date)
            throw new InvalidQuotationLineException("A quotation's valid-until date cannot be before its own date.");

        var quotation = new Quotation
        {
            CustomerId = customerId,
            CurrencyId = currencyId,
            Rate = rate,
            Date = quotationDate,
            ValidUntil = validUntil,
            LifecycleStatus = QuotationStatus.Draft,
            CreateUserId = createUserId,
            CreateDate = createDate,
            BranchId = branchId,
            Notes = notes
        };

        quotation.Raise(new QuotationCreatedDomainEvent(quotation.Id, customerId));
        return quotation;
    }

    // ----- Line mutation (Draft only) -----

    public QuotationLine AddLine(
        long productId, string productName, long unitId, decimal quantity, decimal unitPrice,
        decimal discountAmount = 0, decimal taxAmount = 0, DateTime? requestedDeliveryDate = null, string? notes = null)
    {
        EnsureEditable();

        if (quantity <= 0)
            throw new InvalidQuotationLineException("A quotation line's quantity must be greater than zero.");
        if (unitPrice < 0)
            throw new InvalidQuotationLineException("A quotation line's unit price cannot be negative.");
        if (discountAmount < 0 || taxAmount < 0)
            throw new InvalidQuotationLineException("A quotation line's discount/tax amount cannot be negative.");

        var line = new QuotationLine(productId, productName, unitId, quantity, unitPrice, discountAmount, taxAmount, requestedDeliveryDate, notes)
        {
            QuotationId = Id
        };
        _lines.Add(line);
        RecalculateTotals();
        return line;
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

    // ----- Lifecycle -----

    /// <summary>Draft -> Sent. Requires at least one line (brief §66 "cannot submit empty quotation").</summary>
    public void Send()
    {
        if (LifecycleStatus != QuotationStatus.Draft)
            throw new QuotationNotSendableException("Only a Draft quotation can be sent.");
        if (_lines.Count == 0)
            throw new QuotationNotSendableException("Cannot send a quotation with no lines.");

        LifecycleStatus = QuotationStatus.Sent;
        Raise(new QuotationSentDomainEvent(Id));
    }

    /// <summary>Sent -> Accepted.</summary>
    public void Accept()
    {
        EnsureOpen();

        LifecycleStatus = QuotationStatus.Accepted;
        Raise(new QuotationAcceptedDomainEvent(Id));
    }

    /// <summary>Sent -> Rejected.</summary>
    public void Reject()
    {
        EnsureOpen();

        LifecycleStatus = QuotationStatus.Rejected;
        Raise(new QuotationRejectedDomainEvent(Id));
    }

    /// <summary>Sent -> Expired. The caller supplies "as of" rather than the aggregate reading the
    /// clock itself, keeping the transition deterministic/testable — same shape as
    /// Advances.Domain.Custody.IsOverdue's asOfDate parameter.</summary>
    public void Expire(DateTime asOfDate)
    {
        EnsureOpen();
        if (asOfDate.Date < ValidUntil.Date)
            throw new QuotationNotYetExpiredException("Cannot expire a quotation before its ValidUntil date has passed.");

        LifecycleStatus = QuotationStatus.Expired;
        Raise(new QuotationExpiredDomainEvent(Id));
    }

    /// <summary>Draft or Sent -> Cancelled. Never valid once Accepted/Converted/Rejected/Expired —
    /// those are final commercial outcomes.</summary>
    public void Cancel()
    {
        if (LifecycleStatus == QuotationStatus.Cancelled)
            return;
        if (LifecycleStatus is not (QuotationStatus.Draft or QuotationStatus.Sent))
            throw new QuotationCannotBeCancelledException($"Cannot cancel a quotation that is {LifecycleStatus}.");

        LifecycleStatus = QuotationStatus.Cancelled;
        Raise(new QuotationCancelledDomainEvent(Id));
    }

    /// <summary>
    /// Accepted -> Converted. Called by Sales.Application AFTER it has created the resulting
    /// SalesOrder (brief §12) — this method only records that the conversion happened and which
    /// SalesOrder resulted, it never creates one itself. Guarding on LifecycleStatus == Accepted
    /// (not just "not yet Converted") is what makes a duplicate/retried conversion request fail
    /// rather than silently succeed twice (brief §47 idempotency, brief §66 "cannot convert twice").
    /// </summary>
    public void MarkConverted(long salesOrderId)
    {
        if (LifecycleStatus != QuotationStatus.Accepted)
            throw new QuotationCannotBeConvertedException("Only an Accepted quotation can be converted to a sales order.");
        if (salesOrderId <= 0)
            throw new InvalidQuotationLineException("A valid sales order id is required to convert a quotation.");

        ConvertedToSalesOrderId = salesOrderId;
        LifecycleStatus = QuotationStatus.Converted;
        Raise(new QuotationConvertedDomainEvent(Id, salesOrderId));
    }

    private void RecalculateTotals()
    {
        Subtotal = _lines.Sum(l => l.Quantity * l.UnitPrice);
        DiscountAmount = _lines.Sum(l => l.DiscountAmount);
        TaxAmount = _lines.Sum(l => l.TaxAmount);
        TotalAmount = Subtotal - DiscountAmount + TaxAmount;
    }

    private void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    private void EnsureEditable()
    {
        if (LifecycleStatus != QuotationStatus.Draft)
            throw new QuotationNotEditableException("Only a Draft quotation can be edited.");
    }

    private void EnsureOpen()
    {
        if (LifecycleStatus != QuotationStatus.Sent)
            throw new QuotationNotOpenException($"Cannot act on a quotation that is {LifecycleStatus} — only a Sent quotation can be accepted, rejected, or expired.");
    }
}
