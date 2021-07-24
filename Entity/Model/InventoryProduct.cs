using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("InventoryProduct")]
    public class InventoryProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [Required]
        public long InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }

        [Required]
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public long UnitId { get; set; }

        public virtual Unit Unit { get; set; }               

        public decimal CalcBalance { get; set; }

        public decimal ActualBalance { get; set; }

        public decimal DiffQuantity { get; set; }

        public string Notes { get; set; }                       
    }
}