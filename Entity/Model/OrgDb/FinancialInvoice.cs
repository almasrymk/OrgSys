using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("FinancialInvoice")]
    public class FinancialInvoice : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [ForeignKey("Financial")]
        public long FinancialId { get; set; }

        public Financial Financial { get; set; }

        [ForeignKey("Invoice")]
        public long? InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        public decimal Amount { get; set; }        
    }
}