namespace FixedAssets.Domain;

[Table("DepreciationEntry")]
public class DepreciationEntry : BaseModel
{
    public virtual long AssetId { get; private set; }

    public virtual int PeriodYear { get; private set; }

    public virtual int PeriodMonth { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal Amount { get; private set; }

    public virtual long? JournalId { get; private set; }

    public virtual DateTime PostedAt { get; private set; }

    public virtual FixedAsset? Asset { get; private set; }

    protected DepreciationEntry() { }

    internal DepreciationEntry(long assetId, int periodYear, int periodMonth, decimal amount, DateTime postedAt)
    {
        AssetId = assetId;
        PeriodYear = periodYear;
        PeriodMonth = periodMonth;
        Amount = amount;
        PostedAt = postedAt;
    }

    public void AttachJournal(long journalId)
    {
        if (journalId <= 0)
            throw new Exceptions.FixedAssetDomainException("A journal is required.");
        JournalId = journalId;
    }
}
