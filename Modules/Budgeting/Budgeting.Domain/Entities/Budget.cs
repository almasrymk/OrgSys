namespace Budgeting.Domain;

[Table("Budget")]
public class Budget : BaseModel
{
    private readonly List<BudgetLine> _lines = [];

    [StringLength(150, MinimumLength = 3)]
    public virtual string? Name { get; private set; }

    public virtual long FiscalYearId { get; private set; }

    public virtual long? DepartmentId { get; private set; }

    public virtual DateTime PeriodStart { get; private set; }

    public virtual DateTime PeriodEnd { get; private set; }

    public virtual IReadOnlyCollection<BudgetLine> Lines => _lines.AsReadOnly();

    protected Budget() { }

    public static Budget Create(string name, long fiscalYearId, DateTime periodStart, DateTime periodEnd, long? departmentId)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3)
            throw new Exceptions.BudgetDomainException("Budget name must be at least 3 characters.");
        if (fiscalYearId <= 0)
            throw new Exceptions.BudgetDomainException("A fiscal year is required.");
        if (periodEnd < periodStart)
            throw new Exceptions.BudgetDomainException("Period end cannot be before period start.");

        return new Budget
        {
            Name = name.Trim(),
            FiscalYearId = fiscalYearId,
            DepartmentId = departmentId,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd
        };
    }

    public BudgetLine AddLine(long accountId, decimal amount)
    {
        if (accountId <= 0)
            throw new Exceptions.BudgetDomainException("An account is required.");
        if (amount < 0)
            throw new Exceptions.BudgetDomainException("Budget amount cannot be negative.");

        var line = new BudgetLine(Id, accountId, amount);
        _lines.Add(line);
        return line;
    }
}
