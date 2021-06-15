using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Order")]
    public class Order : BaseModel
    {
        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }
      
        public long? TableId { get; set; }
        public virtual Table Table { get; set; }
        public bool CloseTable { get; set; }
        [Required]
        public DateTime Date { get; set; }
        
        public long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }
        
        public long? ShiftId { get; set; }

        public virtual Shift Shift { get; set; }

        public long? InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }

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
        public virtual List<OrderProduct> OrderProducts { get; set; }
    }
}