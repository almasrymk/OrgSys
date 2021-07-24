using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Financial")]
    public class Financial : MovementModel
    {       
        public long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        [Required]
        public long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public long? OutlayId { get; set; }

        public Outlay Outlay { get; set; }

        [Required]
        public long CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public decimal Rate { get; set; }

        [Required]
        public long SafeId { get; set; }

        public virtual Safe Safe { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountByDefaultCurrency { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public virtual List<FinancialInvoice> FinancialInvoices { get; set; }
    }
}