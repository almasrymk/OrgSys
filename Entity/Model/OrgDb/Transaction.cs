using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Transaction")]
    public class Transaction : MovementModel
    {
        [ForeignKey("Dealer")]
        public long? DealerId { get; set; }

        public Dealer Dealer { get; set; }

        [ForeignKey("Store")]
        public long? StoreId { get; set; }

        public Store Store { get; set; }

        [ForeignKey("ToStore")]
        public long? ToStoreId { get; set; }

        public Store ToStore { get; set; }

        [ForeignKey("Order")]
        public long? OrderId { get; set; }

        public Order Order { get; set; }

        public decimal Total { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public ICollection<TransactionProduct> TransactionProducts { get; set; }
    }
}