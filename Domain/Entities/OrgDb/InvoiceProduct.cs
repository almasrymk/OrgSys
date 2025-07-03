using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("InvoiceProduct")]
    public class InvoiceProduct : BaseEntity
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

        [ForeignKey("Stock")]
        public virtual long? StockId { get; set; }

        public virtual Stock Stock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Service { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Net { get; set; }

        public virtual string Notes { get; set; }                       
    }
}