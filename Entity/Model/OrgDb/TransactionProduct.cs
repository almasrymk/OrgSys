using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("TransactionProduct")]
    public class TransactionProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [ForeignKey("Transaction")]
        public long TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        [ForeignKey("Store")]
        public long? StoreId { get; set; }

        public virtual Store Store { get; set; }

        public decimal Quantity { get; set; }
        
        public decimal Cost { get; set; }

        public decimal Total { get; set; }
        
        public string Notes { get; set; }                       
    }
}