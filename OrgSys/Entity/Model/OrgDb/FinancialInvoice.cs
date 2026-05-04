using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("FinancialInvoice")]
    public class FinancialInvoice : BaseModel
    {
        [Required]
        public virtual long RowNumber { get; set; }

        [ForeignKey("Financial")]
        public virtual long FinancialId { get; set; }

        public virtual Financial Financial { get; set; }

        [ForeignKey("Invoice")]
        public virtual long? InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Amount { get; set; }        
    }
}