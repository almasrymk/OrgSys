namespace FixedAssets.Domain;

[Table("FixedAssetCategory")]
public class FixedAssetCategory : BaseModel
{
    [StringLength(150, MinimumLength = 3)]
    public virtual string? Name { get; private set; }

    public virtual long AssetAccountId { get; private set; }

    public virtual long AccumulatedDepreciationAccountId { get; private set; }

    public virtual long DepreciationExpenseAccountId { get; private set; }

    public virtual int UsefulLifeMonths { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal ResidualPercent { get; private set; }

    protected FixedAssetCategory() { }

    public static FixedAssetCategory Create(
        string name,
        long assetAccountId,
        long accumulatedDepreciationAccountId,
        long depreciationExpenseAccountId,
        int usefulLifeMonths,
        decimal residualPercent)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3)
            throw new Exceptions.FixedAssetDomainException("Category name must be at least 3 characters.");
        if (assetAccountId <= 0 || accumulatedDepreciationAccountId <= 0 || depreciationExpenseAccountId <= 0)
            throw new Exceptions.FixedAssetDomainException("Category must store GL account IDs, not codes.");
        if (usefulLifeMonths <= 0)
            throw new Exceptions.FixedAssetDomainException("Useful life must be at least one month.");
        if (residualPercent < 0 || residualPercent >= 100)
            throw new Exceptions.FixedAssetDomainException("Residual percent must be between 0 and 100.");

        return new FixedAssetCategory
        {
            Name = name.Trim(),
            AssetAccountId = assetAccountId,
            AccumulatedDepreciationAccountId = accumulatedDepreciationAccountId,
            DepreciationExpenseAccountId = depreciationExpenseAccountId,
            UsefulLifeMonths = usefulLifeMonths,
            ResidualPercent = residualPercent
        };
    }
}
