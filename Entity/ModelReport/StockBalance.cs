using System;
using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace Entity.ModelReport
{
    public class StockBalance
    {
        [Key]
        public long StockId { get; set; }
        public string StockName { get; set; }
        public decimal OpenningBalance { get; set; }
        public decimal Balance { get; set; }      
    }
}