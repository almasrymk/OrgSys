namespace Sales.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// One line within a Quotation. ProductId/UnitId are opaque references to Inventory.Domain.Product/
/// MasterData.Domain.Unit — no navigation (brief §6/§8). ProductName is a commercial snapshot taken
/// at the moment the line is added, so a historical quotation's wording never changes if the
/// product's master-data name changes later (brief §8/§35). No public constructor — only reachable
/// through Quotation.AddLine, mirroring Payables.Domain.SupplierPaymentApplicationLine.
/// </summary>
[Table("SalesQuotationLine")]
public class QuotationLine : BaseModel
{
    /// <summary>EF materialization constructor only.</summary>
    protected QuotationLine() { }

    internal QuotationLine(
        long productId, string productName, long unitId, decimal quantity, decimal unitPrice,
        decimal discountAmount, decimal taxAmount, DateTime? requestedDeliveryDate, string? notes)
    {
        ProductId = productId;
        ProductName = productName;
        UnitId = unitId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountAmount = discountAmount;
        TaxAmount = taxAmount;
        RequestedDeliveryDate = requestedDeliveryDate;
        Notes = notes;
        RecalculateLineTotal();
    }

    [ForeignKey(nameof(Quotation))]
    public virtual long QuotationId { get; internal set; }

    public virtual Quotation? Quotation { get; set; }

    public virtual long ProductId { get; private set; }

    [Required, StringLength(200)]
    public virtual string ProductName { get; private set; } = string.Empty;

    public virtual long UnitId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Quantity { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal UnitPrice { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal DiscountAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TaxAmount { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal LineTotal { get; private set; }

    public virtual DateTime? RequestedDeliveryDate { get; private set; }

    [StringLength(500)]
    public virtual string? Notes { get; private set; }

    private void RecalculateLineTotal() => LineTotal = (Quantity * UnitPrice) - DiscountAmount + TaxAmount;
}
