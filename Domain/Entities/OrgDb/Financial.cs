using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Financial")]
    public class Financial : MovementModel
    {
        [ForeignKey("Dealer")]
        public virtual long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        [ForeignKey("PaymentType")]
        public virtual long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        [ForeignKey("Outlay")]
        public virtual long? OutlayId { get; set; }

        public virtual Outlay Outlay { get; set; }

        [ForeignKey("CurrencyId")]
        public long CurrencyId { get; set; }

        public Currency Currency { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [ForeignKey("Safe")]
        public long SafeId { get; set; }

        public virtual Safe Safe { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountByDefaultCurrency { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
        
        public ICollection<FinancialInvoice> FinancialInvoices { get; set; }
    }
}