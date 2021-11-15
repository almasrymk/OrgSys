using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Inventory")]
    public class Inventory : MovementModel
    {      
        public long? UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("Store")]
        public long? StoreId { get; set; }

        public Store Store { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public bool Review { get; set; }

        public bool Closed { get; set; }

        public  ICollection<InventoryProduct> InventoryProducts { get; set; }
    }
}