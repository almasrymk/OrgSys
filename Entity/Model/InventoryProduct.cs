using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("InventoryProduct")]
    public class InventoryProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [Required]
        public long InventoryStoreId { get; set; }

        public virtual InventoryStore InventoryStore { get; set; }

        [Required]
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public long UnitId { get; set; }

        public virtual Unit Unit { get; set; }               

        public decimal CalcBalance { get; set; }
        public decimal ActualBalance { get; set; }
        public decimal DiffQuantity { get; set; }

        public string Notes { get; set; }                       
    }
}