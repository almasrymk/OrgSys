using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Financial")]
    public class Financial : BaseModel
    {
        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }
       
        public long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }
        [Required]
        public long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }
        [Required]
        public long SafeId { get; set; }

        public virtual Safe Safe { get; set; }

        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public virtual List<FinancialInvoice> FinancialInvoices { get; set; }
    }
}