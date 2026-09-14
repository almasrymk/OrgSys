namespace Sales.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// One line within a SalesOrder. ProductId/UnitId are opaque references, ProductName is a
/// commercial snapshot — same reasoning as QuotationLine. DeliveredQuantity/ReturnedQuantity/
/// CancelledQuantity are this aggregate's own authoritative running totals of what has happened to
/// this line so far (brief §14/§47: "do not trust a quantity sent from UI" — they change only
/// through SalesOrder.RecordDelivery/RecordReturn/Cancel, called by Sales.Application AFTER the
/// owning SalesDelivery/SalesReturn aggregate — a future phase — has confirmed its own fact). This
/// is not a competing source of truth: SalesDelivery/SalesReturn remain the source of truth for
/// their OWN document facts (dates, warehouse, references); this is SalesOrder's own summary of
/// "how much of MY order has been fulfilled," the same relationship
/// Advances.Domain.Custody.OutstandingAmount has to Treasury's Financial records. No public
/// constructor — only reachable through SalesOrder.AddLine.
/// </summary>
[Table("SalesOrderLine")]
public class SalesOrderLine : BaseModel
{
    /// <summary>EF materialization constructor only.</summary>
    protected SalesOrderLine() { }

    internal SalesOrderLine(
        long productId, string productName, long unitId, decimal orderedQuantity, decimal unitPrice,
        decimal discountAmount, decimal taxAmount, DateTime? requestedDeliveryDate, string? notes)
    {
        ProductId = productId;
        ProductName = productName;
        UnitId = unitId;
        OrderedQuantity = orderedQuantity;
        UnitPrice = unitPrice;
        DiscountAmount = discountAmount;
        TaxAmount = taxAmount;
        RequestedDeliveryDate = requestedDeliveryDate;
        Notes = notes;
        RecalculateLineTotal();
    }

    [ForeignKey(nameof(SalesOrder))]
    public virtual long SalesOrderId { get; internal set; }

    public virtual SalesOrder? SalesOrder { get; set; }

    public virtual long ProductId { get; private set; }

    [Required, StringLength(200)]
    public virtual string ProductName { get; private set; } = string.Empty;

    public virtual long UnitId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal OrderedQuantity { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal UnitPrice { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal DiscountAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TaxAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal LineTotal { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal DeliveredQuantity { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal ReturnedQuantity { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal CancelledQuantity { get; private set; }

    public virtual DateTime? RequestedDeliveryDate { get; private set; }

    [StringLength(500)]
    public virtual string? Notes { get; private set; }

    /// <summary>Ordered minus delivered minus cancelled (brief §65) — what could still be shipped against this line.</summary>
    public decimal RemainingQuantity => OrderedQuantity - DeliveredQuantity - CancelledQuantity;

    /// <summary>Delivered minus already returned (brief §28) — the maximum a further return on this line can take.</summary>
    public decimal ReturnableQuantity => DeliveredQuantity - ReturnedQuantity;

    internal void RecordDelivery(decimal quantity)
    {
        if (quantity > RemainingQuantity)
            throw new InvalidSalesOrderDeliveryException(
                $"Cannot deliver {quantity} on line for product {ProductId} — only {RemainingQuantity} remains.");

        DeliveredQuantity += quantity;
    }

    internal void RecordReturn(decimal quantity)
    {
        if (quantity > ReturnableQuantity)
            throw new InvalidSalesOrderReturnException(
                $"Cannot return {quantity} on line for product {ProductId} — only {ReturnableQuantity} of the delivered quantity has not already been returned.");

        ReturnedQuantity += quantity;
    }

    /// <summary>Cancels part (or, called with RemainingQuantity, all) of this line's remaining
    /// (undelivered, uncancelled) quantity. Never reduces OrderedQuantity or delivered history.</summary>
    internal void CancelPartial(decimal quantity)
    {
        if (quantity > RemainingQuantity)
            throw new InvalidSalesOrderLineException(
                $"Cannot cancel {quantity} on line for product {ProductId} — only {RemainingQuantity} remains.");

        CancelledQuantity += quantity;
    }

    private void RecalculateLineTotal() => LineTotal = (OrderedQuantity * UnitPrice) - DiscountAmount + TaxAmount;
}
