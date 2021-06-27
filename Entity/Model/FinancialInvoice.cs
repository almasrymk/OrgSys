using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("FinancialInvoice")]
    public class FinancialInvoice : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [Required]
        public long FinancialId { get; set; }

        public virtual Financial Financial { get; set; }

        [Required]
        public long InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }

        public decimal Amount { get; set; }        
    }
}