using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("InventoryProduct")]
    public class InventoryProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [ForeignKey("Inventory")]
        public long InventoryId { get; set; }

        public Inventory Inventory { get; set; }

        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }

        public Unit Unit { get; set; }               

        public decimal CalcBalance { get; set; }

        public decimal ActualBalance { get; set; }

        public decimal DiffQuantity { get; set; }

        public string Notes { get; set; }                       
    }
}