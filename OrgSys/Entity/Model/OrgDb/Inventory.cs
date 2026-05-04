using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Inventory")]
    public class Inventory : MovementModel
    {      
        public virtual long? UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Stock")]
        public virtual long? StockId { get; set; }

        public virtual Stock Stock { get; set; }

        [StringLength(500)]
        public virtual string Notes { get; set; }
         
        public virtual bool Closed { get; set; }

        public virtual ICollection<InventoryProduct> InventoryProducts { get; set; }
    }
}