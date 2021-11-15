using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Invoice")]
    public class Invoice : MovementModel
    {
        [ForeignKey("Dealer")]
        public long DealerId { get; set; }

        public Dealer Dealer { get; set; }

        [ForeignKey("PaymentType")]
        public long PaymentTypeId { get; set; }

        public PaymentType PaymentType { get; set; }

        [ForeignKey("Store")]
        public long? StoreId { get; set; }

        public Store Store { get; set; }

        [ForeignKey("Transaction")]
        public long? TransactionId { get; set; }

        public Transaction Transaction { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public int DiscountType { get; set; }

        public decimal Tax { get; set; }

        public int TaxType { get; set; }

        public decimal Service { get; set; }

        public int ServiceType { get; set; }

        [ForeignKey("Currency")]
        public long CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public decimal Rate { get; set; }

        public decimal Net { get; set; }

        public decimal NetByDefaultCurrency { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public decimal Remaining { get; set; }

        public decimal Paid { get; set; }

        public decimal Credit { get; set; }

        public decimal CreditByDefaultCurrency { get; set; }

        public ICollection<InvoiceProduct> InvoiceProducts { get; set; }
    }
}