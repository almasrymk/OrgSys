namespace Sales.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// SalesOrder — the core Sales aggregate: a customer's confirmed commitment to buy, and the single
/// source of truth for how much of it has been delivered/returned/cancelled so far (brief §13-16).
/// CustomerId is an opaque reference to Parties.Domain.Dealer, same naming reasoning as
/// Quotation.CustomerId. SalesOrder never touches Inventory stock or creates a GeneralLedger/
/// AccountsReceivable entity itself (brief §4/§6) — Confirm/RecordDelivery/RecordReturn only record
/// facts on this aggregate; Sales.Application is responsible for coordinating the actual Inventory
/// reservation/stock-issue and AccountsReceivable billing through their own Contracts, the same
/// arm's-length shape Advances.Domain.Custody uses for Treasury. Mirrors Custody's aggregate shape
/// throughout (private setters, protected EF ctor, static Create factory, manual domain-event list).
/// </summary>
[Table("SalesOrder")]
public class SalesOrder : MovementModel
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private readonly List<SalesOrderLine> _lines = [];

    [NotMapped]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public virtual IReadOnlyCollection<SalesOrderLine> Lines => _lines.AsReadOnly();

    /// <summary>EF materialization constructor only. Business code creates a SalesOrder through
    /// <see cref="Create"/>.</summary>
    protected SalesOrder() { }

    public virtual long CustomerId { get; private set; }

    public virtual DateTime? RequestedDeliveryDate { get; private set; }

    public virtual long CurrencyId { get; private set; }

    [Column(TypeName = "decimal(18,6)")]
    public virtual decimal Rate { get; private set; }

    /// <summary>Set only when this order was created by converting a Quotation (brief §12) —
    /// reference id only, no navigation. Null for a direct sales order.</summary>
    public virtual long? SourceQuotationId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Subtotal { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal DiscountAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TaxAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TotalAmount { get; private set; }

    public virtual SalesOrderStatus LifecycleStatus { get; private set; }

    [StringLength(500)]
    public virtual string? Notes { get; private set; }

    // ----- Construction -----

    public static SalesOrder Create(
        long customerId,
        long currencyId,
        decimal rate,
        DateTime orderDate,
        long createUserId,
        DateTime createDate,
        long? sourceQuotationId = null,
        DateTime? requestedDeliveryDate = null,
        long? branchId = null,
        string? notes = null)
    {
        if (rate <= 0)
            throw new InvalidSalesOrderLineException("A sales order's exchange rate must be greater than zero.");

        var order = new SalesOrder
        {
            CustomerId = customerId,
            CurrencyId = currencyId,
            Rate = rate,
            Date = orderDate,
            SourceQuotationId = sourceQuotationId,
            RequestedDeliveryDate = requestedDeliveryDate,
            LifecycleStatus = SalesOrderStatus.Draft,
            CreateUserId = createUserId,
            CreateDate = createDate,
            BranchId = branchId,
            Notes = notes
        };

        order.Raise(new SalesOrderCreatedDomainEvent(order.Id, customerId));
        return order;
    }

    // ----- Line mutation (Draft only) -----

    public SalesOrderLine AddLine(
        long productId, string productName, long unitId, decimal orderedQuantity, decimal unitPrice,
        decimal discountAmount = 0, decimal taxAmount = 0, DateTime? requestedDeliveryDate = null, string? notes = null)
    {
        EnsureEditable();

        if (orderedQuantity <= 0)
            throw new InvalidSalesOrderLineException("A sales order line's quantity must be greater than zero.");
        if (unitPrice < 0)
            throw new InvalidSalesOrderLineException("A sales order line's unit price cannot be negative.");
        if (discountAmount < 0 || taxAmount < 0)
            throw new InvalidSalesOrderLineException("A sales order line's discount/tax amount cannot be negative.");

        var line = new SalesOrderLine(productId, productName, unitId, orderedQuantity, unitPrice, discountAmount, taxAmount, requestedDeliveryDate, notes)
        {
            SalesOrderId = Id
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

    /// <summary>Draft -> Confirmed. Requires at least one line (brief §16 "cannot confirm empty order").
    /// From here, Customer/currency/lines can no longer change (brief §15/§16) — there is no
    /// UpdateHeader method that would let them.</summary>
    public void Confirm()
    {
        if (LifecycleStatus != SalesOrderStatus.Draft)
            throw new SalesOrderNotConfirmableException("Only a Draft sales order can be confirmed.");
        if (_lines.Count == 0)
            throw new SalesOrderNotConfirmableException("Cannot confirm a sales order with no lines.");

        LifecycleStatus = SalesOrderStatus.Confirmed;
        Raise(new SalesOrderConfirmedDomainEvent(Id));
    }

    /// <summary>
    /// Records that <paramref name="quantity"/> of <paramref name="lineId"/> was delivered — called
    /// by Sales.Application AFTER the owning SalesDelivery (a future phase) has confirmed the actual
    /// fulfillment; this method never issues stock itself (brief §17/§21). Rejects delivery beyond
    /// what remains on the line (brief §16/§20 over-delivery prevention) and on an order not open
    /// for delivery at all (brief §16 "cannot deliver unconfirmed/cancelled order").
    /// </summary>
    public void RecordDelivery(long lineId, decimal quantity)
    {
        EnsureOpenForDelivery();

        if (quantity <= 0)
            throw new InvalidSalesOrderDeliveryException("A delivered quantity must be greater than zero.");

        var line = FindLine(lineId, "delivered");
        line.RecordDelivery(quantity);

        RecalculateFulfillmentStatus();
    }

    /// <summary>
    /// Records that <paramref name="quantity"/> of <paramref name="lineId"/> was returned — called by
    /// Sales.Application AFTER the owning SalesReturn (a future phase) has confirmed the physical
    /// return; never accepts inventory itself (brief §29). Cumulative returns on a line cannot
    /// exceed what was actually delivered minus what was already returned (brief §27/§28) —
    /// enforced by SalesOrderLine.ReturnableQuantity, which also rejects returning an item that was
    /// never delivered in the first place.
    /// </summary>
    public void RecordReturn(long lineId, decimal quantity)
    {
        if (LifecycleStatus == SalesOrderStatus.Draft)
            throw new InvalidSalesOrderReturnException("Cannot return against a sales order that was never confirmed/delivered.");
        if (quantity <= 0)
            throw new InvalidSalesOrderReturnException("A returned quantity must be greater than zero.");

        var line = FindLine(lineId, "returned");
        line.RecordReturn(quantity);
    }

    /// <summary>
    /// Cancels the remaining (undelivered) quantity of a single line — a partial cancellation
    /// (brief §42: e.g. Ordered 100, Delivered 40, cancel 30 of the remaining 60, leaving 30 still
    /// deliverable). Never reduces OrderedQuantity itself; delivered history is untouched.
    /// </summary>
    public void CancelLine(long lineId, decimal quantity)
    {
        EnsureCancellable();

        if (quantity <= 0)
            throw new InvalidSalesOrderLineException("A line cancellation quantity must be greater than zero.");

        var line = FindLine(lineId, "cancelled");
        if (quantity > line.RemainingQuantity)
            throw new InvalidSalesOrderLineException($"Cannot cancel {quantity} on line for product {line.ProductId} — only {line.RemainingQuantity} remains.");

        line.CancelPartial(quantity);
        RecalculateFulfillmentStatus();
    }

    /// <summary>
    /// Cancels the ENTIRE order's remaining undelivered quantity across every line (brief §41).
    /// If nothing was ever delivered, this is a clean cancellation (SalesOrderCancelledDomainEvent).
    /// If some lines already have delivered history, that history is preserved — the order becomes
    /// closed-out (nothing left to deliver) rather than erased, raising
    /// SalesOrderCompletedDomainEvent instead (see SalesOrderStatus's remark on why there is no
    /// separate status for this case).
    /// </summary>
    public void Cancel()
    {
        EnsureCancellable();

        foreach (var line in _lines.Where(l => l.RemainingQuantity > 0))
            line.CancelPartial(line.RemainingQuantity);

        RecalculateFulfillmentStatus();
    }

    private void RecalculateTotals()
    {
        Subtotal = _lines.Sum(l => l.OrderedQuantity * l.UnitPrice);
        DiscountAmount = _lines.Sum(l => l.DiscountAmount);
        TaxAmount = _lines.Sum(l => l.TaxAmount);
        TotalAmount = Subtotal - DiscountAmount + TaxAmount;
    }

    /// <summary>Recomputes LifecycleStatus from the lines' own Delivered/Remaining totals after any
    /// delivery or cancellation — the aggregate never lets a handler set Status directly (brief §16).</summary>
    private void RecalculateFulfillmentStatus()
    {
        var totalDelivered = _lines.Sum(l => l.DeliveredQuantity);
        var totalRemaining = _lines.Sum(l => l.RemainingQuantity);

        if (totalRemaining == 0)
        {
            var wasAlreadyClosed = LifecycleStatus is SalesOrderStatus.Delivered or SalesOrderStatus.Cancelled;
            LifecycleStatus = totalDelivered == 0 ? SalesOrderStatus.Cancelled : SalesOrderStatus.Delivered;

            if (!wasAlreadyClosed)
                Raise(totalDelivered == 0
                    ? new SalesOrderCancelledDomainEvent(Id)
                    : new SalesOrderCompletedDomainEvent(Id));
        }
        else
        {
            LifecycleStatus = totalDelivered > 0 ? SalesOrderStatus.PartiallyDelivered : LifecycleStatus;
        }
    }

    private SalesOrderLine FindLine(long lineId, string action) =>
        _lines.FirstOrDefault(l => l.Id == lineId)
        ?? throw new InvalidSalesOrderLineException($"Line {lineId} does not belong to this sales order and cannot be {action}.");

    private void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    private void EnsureEditable()
    {
        if (LifecycleStatus != SalesOrderStatus.Draft)
            throw new SalesOrderNotEditableException("Only a Draft sales order can be edited.");
    }

    private void EnsureOpenForDelivery()
    {
        if (LifecycleStatus is not (SalesOrderStatus.Confirmed or SalesOrderStatus.PartiallyDelivered))
            throw new InvalidSalesOrderDeliveryException($"Cannot record a delivery against a sales order that is {LifecycleStatus}.");
    }

    private void EnsureCancellable()
    {
        if (LifecycleStatus is SalesOrderStatus.Delivered or SalesOrderStatus.Cancelled)
            throw new SalesOrderCannotBeCancelledException($"Cannot cancel a sales order that is already {LifecycleStatus}.");
    }
}
