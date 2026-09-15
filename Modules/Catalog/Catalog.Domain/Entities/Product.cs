namespace Catalog.Domain
{
    [Table("Product")]
    public class Product : BaseModel
    {
        [Required]
        public virtual string Name { get; set; } = null!;

        public virtual string? Nickname { get; set; }
       
        public virtual string? Barcode { get; set; }

        public virtual string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Cost { get; set; }

        [ForeignKey("Classification")]
        public virtual long ClassificationId { get; set; }

        public virtual Classification? Classification { get; set; }

        [ForeignKey("Dealer")]
        public virtual long? DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; }

        public virtual string? Recipe { get; set; }

        /// <summary>Additive (brief §18/§52) — how this item's stock must be tracked. Fixed once
        /// movements exist for the item; enforced by Inventory.Application, not here (this entity
        /// keeps its existing AutoMapper-CRUD shape rather than becoming a Custody-style factory
        /// aggregate — see docs/ddd/inventory-target-architecture.md §2 for why).</summary>
        public virtual TrackingType TrackingType { get; set; } = TrackingType.None;

        /// <summary>Additive (brief §20-22). Defaults to WeightedAverage, matching the item's
        /// existing de facto behavior (a flat, manually-entered Cost).</summary>
        public virtual CostingMethod CostingMethod { get; set; } = CostingMethod.WeightedAverage;

        /// <summary>Additive (brief §18) — kept separate from TrackingType.Batch: a batch can be
        /// required for traceability without expiry mattering (e.g. supplier-lot tracking of a
        /// non-perishable item).</summary>
        public virtual bool ExpiryTracking { get; set; }

        /// <summary>Additive — an inactive item is rejected by new transactional document lines
        /// (brief §52 ItemInactiveException), same pattern as Stock.IsActive below.</summary>
        public virtual bool IsActive { get; set; } = true;

        /// <summary>Additive (Catalog Phase 2) — what this Product represents for stock/commercial
        /// purposes. Backfilled from TrackingType on relocation; see
        /// docs/catalog/catalog-data-migration-plan.md §2. Application-layer invariant: TrackingType
        /// must be None when ProductType is Service (enforced in Catalog.Application, matching the
        /// existing convention on TrackingType/CostingMethod below).</summary>
        public virtual ProductType ProductType { get; set; } = ProductType.StockItem;

        /// <summary>Additive (Catalog Phase 6) — optional, a Product may have no Brand (brief §28
        /// "No Brand" edge case).</summary>
        [ForeignKey("Brand")]
        public virtual long? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }

        public virtual ICollection<ProductUnit>? ProductUnits { get; set; }

        public virtual ICollection<ProductPropertyElement>? ProductPropertyElements { get; set; }
    }
}