using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Invoice")]
    public class Invoice : MovementModel
    {
        [ForeignKey("Dealer")]
        public virtual long DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        [ForeignKey("PaymentType")]
        public virtual long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        [ForeignKey("Stock")]
        public virtual long? StockId { get; set; }

        public virtual Stock Stock { get; set; }

        [ForeignKey("Transaction")]
        public virtual long? TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        public virtual decimal Total { get; set; }

        public virtual decimal Discount { get; set; }

        public virtual int DiscountType { get; set; }

        public virtual decimal Tax { get; set; }

        public virtual int TaxType { get; set; }

        public virtual decimal Service { get; set; }

        public virtual int ServiceType { get; set; }

        [ForeignKey("Currency")]
        public virtual long CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual decimal Net { get; set; }

        public virtual decimal NetByDefaultCurrency { get; set; }

        [StringLength(500)]
        public virtual string Notes { get; set; }

        public virtual decimal Remaining { get; set; }

        public virtual decimal Paid { get; set; }

        public virtual decimal Credit { get; set; }

        public virtual decimal CreditByDefaultCurrency { get; set; }          

        public virtual ICollection<InvoiceProduct> InvoiceProducts { get; set; }
    }
}