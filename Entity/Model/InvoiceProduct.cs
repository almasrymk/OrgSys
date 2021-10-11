using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("InvoiceProduct")]
    public class InvoiceProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [Required]
        public long InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }

        [Required]
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public long UnitId { get; set; }

        public virtual Unit Unit { get; set; }
        
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }

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