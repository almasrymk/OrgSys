namespace Treasury.Domain;

[Table("FinancialAccount")]
public class FinancialAccount : BaseModel
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public FinancialAccountType FinancialAccountType { get; set; }

    // No navigation to Accounting.Domain.Account — see Parties.Domain/Entities/Dealer.cs for why.
    public long? AccountId { get; set; }

    /// <summary>Scalar-only MasterData Currency reference. FK preserved in OrgContext.</summary>
    public long? CurrencyId { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual CashBox? CashBox { get; set; }
    public virtual BankAccount? BankAccount { get; set; }
}
