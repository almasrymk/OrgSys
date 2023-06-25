using System;
using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;

namespace Entity.ModelReport
{
    public class StockBalance
    {
        [Key]
        public long StockId { get; set; }
        public string StockName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        
        public long ClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
    }
}