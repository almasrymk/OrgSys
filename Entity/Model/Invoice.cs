using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Invoice")]
    public class Invoice : BaseModel
    {
        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        [Required]
        public long PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        [Required]
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }
     
        public long? ShiftId { get; set; }

        public virtual Shift Shift { get; set; }
        
        public long? TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }
        public decimal Total { get; set; }

        public decimal Discount { get; set; }
        public int DiscountType { get; set; }
        public decimal Tax { get; set; }
        public int TaxType { get; set; }
        public decimal Service { get; set; }
        public int ServiceType { get; set; }


        public decimal Net { get; set; }
        

        [StringLength(500)]
        public string Notes { get; set; }
        public decimal Remaining { get; set; }
        public decimal Paid { get; set; }

        public virtual List<InvoiceProduct> InvoiceProducts { get; set; }
    }
}