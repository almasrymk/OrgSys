namespace Catalog.Domain
{
    /// <summary>
    /// New Catalog-owned aggregate (docs/catalog/catalog-target-architecture.md §2). Deliberately
    /// its own aggregate root — PriceListEntry references Product by id, Product does not hold a
    /// collection of PriceLists — so Pricing can be extracted to its own bounded context later
    /// without touching Product at all (brief's "future pricing extraction" guidance).
    /// </summary>
    [Table("PriceList")]
    public class PriceList : BaseModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public virtual string Name { get; set; } = null!;

        /// <summary>FK -> MasterData.Currency. Kept as a scalar-only reference (no navigation),
        /// matching how every other module references Currency by id where the value is only ever
        /// displayed, not queried through.</summary>
        public virtual long? CurrencyId { get; set; }

        public virtual DateTime? ValidFrom { get; set; }

        public virtual DateTime? ValidTo { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual bool IsActive { get; set; } = true;

        public virtual ICollection<PriceListEntry>? Entries { get; set; }
    }
}
