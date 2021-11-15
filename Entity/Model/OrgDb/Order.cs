using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Order")]
    public class Order : MovementModel
    {
        [ForeignKey("Table")]
        public long? TableId { get; set; }

        public  Table Table { get; set; }

        public bool CloseTable { get; set; }

        [ForeignKey("Dealer")]
        public long? DealerId { get; set; }

        public  Dealer Dealer { get; set; }

        [ForeignKey("Invoice")]
        public long? InvoiceId { get; set; }

        public  Invoice Invoice { get; set; }

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

        public  ICollection<OrderProduct> OrderProducts { get; set; }
    }
}