namespace Payables.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>One (Payable, amount) allocation line within a SupplierPaymentApplication. Mirrors Receivables.Domain.PaymentApplicationLine: no public constructor, only reachable through SupplierPaymentApplication.RecordLine.</summary>
[Table("SupplierPaymentApplicationLine")]
public class SupplierPaymentApplicationLine : BaseModel
{
    /// <summary>EF materialization constructor only.</summary>
    protected SupplierPaymentApplicationLine() { }

    internal SupplierPaymentApplicationLine(long payableId, decimal amount)
    {
        PayableId = payableId;
        Amount = amount;
    }

    [ForeignKey(nameof(SupplierPaymentApplication))]
    public virtual long SupplierPaymentApplicationId { get; internal set; }

    public virtual SupplierPaymentApplication? SupplierPaymentApplication { get; set; }

    public virtual long PayableId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Amount { get; private set; }
}
