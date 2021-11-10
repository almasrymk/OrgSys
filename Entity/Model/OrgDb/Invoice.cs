using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Invoice", Schema = "org")]
    public class Invoice : MovementModel
    {
        [Required]
        public long DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        [Required]
        public long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        [Required]
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }            
        
        public long? TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public int DiscountType { get; set; }

        public decimal Tax { get; set; }

        public int TaxType { get; set; }

        public decimal Service { get; set; }

        public int ServiceType { get; set; }

        [Required]
        public long CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public decimal Rate { get; set; }

        public decimal Net { get; set; }

        public decimal NetByDefaultCurrency { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public decimal Remaining { get; set; }

        public decimal Paid { get; set; }

        public decimal Credit { get; set; }

        public decimal CreditByDefaultCurrency { get; set; }

        public virtual List<InvoiceProduct> InvoiceProducts { get; set; }
    }
}