using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("InvoiceProduct")]
    public class InvoiceProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [ForeignKey("Invoice")]
        public long InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }

        public Unit Unit { get; set; }

        [ForeignKey("Store")]
        public long? StoreId { get; set; }

        public Store Store { get; set; }

        public decimal Quantity { get; set; }
        
        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal Service { get; set; }
        public decimal Net { get; set; }

        public string Notes { get; set; }                       
    }
}