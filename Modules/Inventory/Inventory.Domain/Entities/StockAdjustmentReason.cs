namespace Inventory.Domain
{
    /// <summary>Reason code for a StockAdjustment (brief §12) — e.g. PhysicalCount, Damage, Loss,
    /// Found, Expiry, DataCorrection, OpeningBalanceCorrection, Other. Plain reference data, same
    /// shape as the existing TransactionType/Property master-data entities.</summary>
    [Table("StockAdjustmentReason")]
    public class StockAdjustmentReason : BaseModel
    {
        [Required, StringLength(50)]
        public new virtual string Code { get; set; } = null!;

        [Required, StringLength(100)]
        public virtual string Name { get; set; } = null!;

        public virtual bool IsActive { get; set; } = true;
    }
}
