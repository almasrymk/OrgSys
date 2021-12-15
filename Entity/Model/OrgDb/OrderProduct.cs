using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("OrderProduct")]
    public class OrderProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [ForeignKey("Order")]
        public long OrderId { get; set; }

        public Order Order { get; set; }

        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }

        public Unit Unit { get; set; }               

        public decimal Quantity { get; set; }
        
        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Service { get; set; }

        public decimal Tax { get; set; }

        public decimal Net { get; set; }

        public string Notes { get; set; }                       
    }
}