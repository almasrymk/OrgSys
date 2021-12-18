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

        [ForeignKey("Store")]
        public virtual long? StoreId { get; set; }

        public virtual Store Store { get; set; }

        [ForeignKey("ToStore")]
        public virtual long? ToStoreId { get; set; }

        public virtual Store ToStore { get; set; }

        [ForeignKey("Order")]
        public virtual long? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual decimal Total { get; set; }

        [StringLength(500)]
        public virtual string Notes { get; set; }

        public virtual ICollection<TransactionProduct> TransactionProducts { get; set; }
    }
}