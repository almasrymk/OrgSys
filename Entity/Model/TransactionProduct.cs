using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

namespace Entity.Model
{
    [Table("TransactionProduct")]
    public class TransactionProduct : BaseModel
    {
        [Required]
        public long RowNumber { get; set; }

        [Required]
        public long TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        [Required]
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public long UnitId { get; set; }

        public virtual Unit Unit { get; set; }
        
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }

        public decimal Quantity { get; set; }
        
        public decimal Cost { get; set; }

        public decimal Total { get; set; }
        
        public string Notes { get; set; }                       
    }
}