using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Transaction", Schema = "org")]
    public class Transaction : MovementModel
    {
        public long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }     

        [Required]
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }

        public long? ToStoreId { get; set; }

        public virtual Store ToStore { get; set; }

        public long? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public decimal Total { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public virtual List<TransactionProduct> TransactionProducts { get; set; }
    }
}