namespace FixedAssets.Domain;

[Table("FixedAsset")]
public class FixedAsset : BaseModel
{
    private readonly List<DepreciationEntry> _entries = [];

    [StringLength(150, MinimumLength = 3)]
    public virtual string? Name { get; private set; }

    public virtual long CategoryId { get; private set; }

    public virtual DateTime AcquisitionDate { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal AcquisitionCost { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal ResidualValue { get; private set; }

    public virtual int UsefulLifeMonths { get; private set; }

    public virtual long CurrencyId { get; private set; }

    public virtual long? BranchId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal AccumulatedDepreciation { get; private set; }

    public virtual FixedAssetStatus LifecycleStatus { get; private set; }

    public virtual IReadOnlyCollection<DepreciationEntry> Entries => _entries.AsReadOnly();

    protected FixedAsset() { }

    public static FixedAsset Create(
        string name,
        long categoryId,
        DateTime acquisitionDate,
        decimal acquisitionCost,
        decimal residualValue,
        int usefulLifeMonths,
        long currencyId,
        long? branchId)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3)
            throw new Exceptions.FixedAssetDomainException("Asset name must be at least 3 characters.");
        if (categoryId <= 0)
            throw new Exceptions.FixedAssetDomainException("A category is required.");
        if (acquisitionCost <= 0)
            throw new Exceptions.FixedAssetDomainException("Acquisition cost must be greater than zero.");
        if (residualValue < 0 || residualValue >= acquisitionCost)
            throw new Exceptions.FixedAssetDomainException("Residual value must be less than acquisition cost.");
        if (usefulLifeMonths <= 0)
            throw new Exceptions.FixedAssetDomainException("Useful life must be at least one month.");
        if (currencyId <= 0)
            throw new Exceptions.FixedAssetDomainException("A currency is required.");

        return new FixedAsset
        {
            Name = name.Trim(),
            CategoryId = categoryId,
            AcquisitionDate = acquisitionDate,
            AcquisitionCost = acquisitionCost,
            ResidualValue = residualValue,
            UsefulLifeMonths = usefulLifeMonths,
            CurrencyId = currencyId,
            BranchId = branchId,
            AccumulatedDepreciation = 0,
            LifecycleStatus = FixedAssetStatus.Active
        };
    }

    public decimal MonthlyDepreciationAmount()
    {
        var depreciable = AcquisitionCost - ResidualValue;
        if (UsefulLifeMonths <= 0 || depreciable <= 0)
            return 0;
        return Math.Round(depreciable / UsefulLifeMonths, 2, MidpointRounding.AwayFromZero);
    }

    public decimal RemainingDepreciable() =>
        Math.Max(0, AcquisitionCost - ResidualValue - AccumulatedDepreciation);

    public DepreciationEntry PostStraightLinePeriod(int periodYear, int periodMonth, DateTime postedAt)
    {
        if (LifecycleStatus != FixedAssetStatus.Active)
            throw new Exceptions.FixedAssetDomainException("Only active assets can be depreciated.");
        if (periodMonth is < 1 or > 12)
            throw new Exceptions.FixedAssetDomainException("Period month is invalid.");
        if (_entries.Any(e => e.PeriodYear == periodYear && e.PeriodMonth == periodMonth))
            throw new Exceptions.FixedAssetDomainException("Depreciation for this period was already posted.");

        var amount = Math.Min(MonthlyDepreciationAmount(), RemainingDepreciable());
        if (amount <= 0)
            throw new Exceptions.FixedAssetDomainException("The asset is fully depreciated.");

        AccumulatedDepreciation += amount;
        if (RemainingDepreciable() <= 0)
            LifecycleStatus = FixedAssetStatus.FullyDepreciated;

        var entry = new DepreciationEntry(Id, periodYear, periodMonth, amount, postedAt);
        _entries.Add(entry);
        return entry;
    }
}
