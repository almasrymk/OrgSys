using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Order")]
    public class Order : MovementModel
    {
        [ForeignKey("Table")]
        public virtual long? TableId { get; set; }

        public virtual Table Table { get; set; }

        public virtual bool CloseTable { get; set; }

        [ForeignKey("Dealer")]
        public virtual long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        [ForeignKey("Invoice")]
        public virtual long? InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual decimal Total { get; set; }

        public virtual decimal Discount { get; set; }

        public virtual int DiscountType { get; set; }

        public virtual decimal Tax { get; set; }

        public virtual int TaxType { get; set; }

        public virtual decimal Service { get; set; }

        public virtual int ServiceType { get; set; }

        public virtual decimal Net { get; set; }

        [StringLength(500)]
        public virtual string Notes { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}