using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Transaction")]
    public class Transaction : MovementModel
    {
        [ForeignKey("Dealer")]
        public virtual long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        [ForeignKey("Stock")]
        public virtual long? StockId { get; set; }

        public virtual Stock Stock { get; set; }

        [ForeignKey("ToStock")]
        public virtual long? ToStockId { get; set; }

        public virtual Stock ToStock { get; set; }

        [ForeignKey("Order")]
        public virtual long? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual decimal Total { get; set; }

        [StringLength(500)]
        public virtual string Notes { get; set; }

        public virtual ICollection<TransactionProduct> TransactionProducts { get; set; }
    }
}