namespace Inventory.Domain
{
    [Table("Inventory")]
    public class Inventory : MovementModel
    {      
        // User navigation dropped — Inventory doesn't otherwise depend on Administration.Domain,
        // and this nav was never dereferenced (Fluent "no navigation" config in OrgContext
        // preserves the FK constraint) — see docs/modular-monolith-analysis.md §21.
        public virtual long? UserId { get; set; }

        [ForeignKey("Stock")]
        public virtual long? StockId { get; set; }

        public virtual Stock? Stock { get; set; }

        [StringLength(500)]
        public virtual string? Notes { get; set; }
         
        public virtual bool Closed { get; set; }

        public virtual bool HasAdjustment { get; set; }

        public virtual ICollection<InventoryProduct>? InventoryProducts { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
