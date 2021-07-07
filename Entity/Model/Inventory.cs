using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Inventory")]
    public class Inventory : BaseModel
    {
        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }            
      
        public long? UserId { get; set; }

        public virtual User User { get; set; }
        [Required]
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }
        [StringLength(500)]
        public string Notes { get; set; }
        public bool Review { get; set; }
        public bool Closed { get; set; }

        public virtual List<InventoryProduct> InventoryProducts { get; set; }
    }
}