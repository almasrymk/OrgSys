namespace Receivables.Domain;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>One (Receivable, amount) allocation line within a PaymentApplication. Mirrors Accounting.Domain.JournalItem's shape: no public constructor, only reachable through PaymentApplication.RecordLine.</summary>
[Table("PaymentApplicationLine")]
public class PaymentApplicationLine : BaseModel
{
    /// <summary>EF materialization constructor only.</summary>
    protected PaymentApplicationLine() { }

    internal PaymentApplicationLine(long receivableId, decimal amount)
    {
        ReceivableId = receivableId;
        Amount = amount;
    }

    [ForeignKey(nameof(PaymentApplication))]
    public virtual long PaymentApplicationId { get; internal set; }

    public virtual PaymentApplication? PaymentApplication { get; set; }

    public virtual long ReceivableId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Amount { get; private set; }
}
