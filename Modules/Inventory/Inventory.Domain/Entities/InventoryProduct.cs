namespace Inventory.Domain
{
    [Table("InventoryProduct")]
    public class InventoryProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Inventory")]
        public virtual long InventoryId { get; set; }

        public virtual Inventory? Inventory { get; set; }

        /// <summary>Scalar-only Catalog Product/Unit references. FKs preserved in OrgContext.</summary>
        public virtual long ProductId { get; set; }

        public virtual long UnitId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal CalcBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal ActualBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal DiffQuantity { get; set; }

        public virtual string? Notes { get; set; }                       
    }
}