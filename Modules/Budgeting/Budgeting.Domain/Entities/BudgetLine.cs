namespace Budgeting.Domain;

[Table("BudgetLine")]
public class BudgetLine : BaseModel
{
    public virtual long BudgetId { get; private set; }

    public virtual long AccountId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Amount { get; private set; }

    public virtual Budget? Budget { get; private set; }

    protected BudgetLine() { }

    internal BudgetLine(long budgetId, long accountId, decimal amount)
    {
        BudgetId = budgetId;
        AccountId = accountId;
        Amount = amount;
    }
}
