using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("Order")]
    public class Order : BaseModel
    {
        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        public long TableId { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal Net { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }             
    }
}