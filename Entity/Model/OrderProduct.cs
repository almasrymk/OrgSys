using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("OrderProduct")]
    public class OrderProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }        

        [Required]
        public long OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Required]
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public long UnitId { get; set; }

        public virtual Unit Unit { get; set; }               

        public decimal Quantity { get; set; }
        
        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal Net { get; set; }

        public string Notes { get; set; }                       
    }
}