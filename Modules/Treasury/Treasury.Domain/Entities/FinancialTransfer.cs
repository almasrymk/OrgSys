namespace Treasury.Domain;

[Table("FinancialTransfer")]
public class FinancialTransfer : MovementModel
{
    [ForeignKey(nameof(FromFinancialAccount))]
    public long FromFinancialAccountId { get; set; }
    public virtual FinancialAccount? FromFinancialAccount { get; set; }

    [ForeignKey(nameof(ToFinancialAccount))]
    public long ToFinancialAccountId { get; set; }
    public virtual FinancialAccount? ToFinancialAccount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [ForeignKey(nameof(Currency))]
    public long CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }

    [Column(TypeName = "decimal(18,6)")]
    public decimal ExchangeRate { get; set; } = 1;

    [StringLength(500)]
    public string? Description { get; set; }

    public virtual ICollection<Financial>? Transactions { get; set; }
}
