using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("InventoryProduct")]
    public class InventoryProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Inventory")]
        public virtual long InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }

        [ForeignKey("Product")]
        public virtual long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal CalcBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal ActualBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal DiffQuantity { get; set; }

        public virtual string Notes { get; set; }                       
    }
}