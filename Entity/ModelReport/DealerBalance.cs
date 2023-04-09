using System;
using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace Entity.ModelReport
{
    public class DealerBalance
    {
        [Key]
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerImgPath { get; set; }
        public decimal OpenningBalance { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal TotalReturnInvoice { get; set; }
        public decimal TotalCreditInvoice { get; set; }
        public decimal TotalPaidInvoice { get; set; }
    }
}