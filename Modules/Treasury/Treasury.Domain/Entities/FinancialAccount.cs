namespace Treasury.Domain;

[Table("FinancialAccount")]
public class FinancialAccount : BaseModel
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public FinancialAccountType FinancialAccountType { get; set; }

    [ForeignKey(nameof(Account))]
    public long? AccountId { get; set; }
    public virtual Account? Account { get; set; }

    [ForeignKey(nameof(Currency))]
    public long? CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual CashBox? CashBox { get; set; }
    public virtual BankAccount? BankAccount { get; set; }
}
