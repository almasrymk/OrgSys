using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("InvoiceProduct")]
    public class InvoiceProduct : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Invoice")]
        public virtual long InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }

        [ForeignKey("Product")]
        public virtual long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        [ForeignKey("Store")]
        public virtual long? StoreId { get; set; }

        public virtual Store Store { get; set; }

        public virtual decimal Quantity { get; set; }
        
        public virtual decimal Price { get; set; }

        public virtual decimal Total { get; set; }

        public virtual decimal Discount { get; set; }

        public virtual decimal Tax { get; set; }

        public virtual decimal Service { get; set; }

        public virtual decimal Net { get; set; }

        public virtual string Notes { get; set; }                       
    }
}