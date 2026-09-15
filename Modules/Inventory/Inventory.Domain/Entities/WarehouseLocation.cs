namespace Inventory.Domain
{
    /// <summary>
    /// A physical bin/zone/aisle/rack within a Warehouse (Stock) — brief §4.2. Kept optional: nothing
    /// requires a LocationId to be set, so existing Stock-only movements/balances keep working
    /// unchanged (LocationId is nullable everywhere it's referenced). Modeled the same plain
    /// public-setter BaseModel shape as the existing Stock/Property master-data entities (not the
    /// Custody-style factory+private-setter shape) since it is reference data consumed by the same
    /// generic CreateCommandHandler&lt;TCommand,TEntity&gt; CQRS the rest of Inventory.Application's
    /// master-data aggregates already use — see docs/ddd/inventory-target-architecture.md §3.
    /// </summary>
    [Table("WarehouseLocation")]
    public class WarehouseLocation : BaseModel
    {
        [ForeignKey("Stock")]
        public virtual long StockId { get; set; }

        public virtual Stock? Stock { get; set; }

        [Required, StringLength(50)]
        public new virtual string Code { get; set; } = null!;

        [StringLength(100)]
        public virtual string? Name { get; set; }

        [ForeignKey("ParentLocation")]
        public virtual long? ParentLocationId { get; set; }

        public virtual WarehouseLocation? ParentLocation { get; set; }

        /// <summary>Free-text zone/aisle/rack/bin classification — brief §4.2 lists a hierarchy as an
        /// example, not a fixed enum, since OrgSys does not need a full WMS today. ParentLocationId
        /// alone already models any depth of hierarchy the business needs.</summary>
        [StringLength(50)]
        public virtual string? LocationType { get; set; }

        public virtual bool IsReceivable { get; set; } = true;

        public virtual bool IsPickable { get; set; } = true;

        public virtual bool IsActive { get; set; } = true;
    }
}
