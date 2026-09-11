using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;

namespace Reporting.Application
{
    public class DealerBalance
    {
        [Key]
        public long DealerId { get; set; }
        public string? DealerName { get; set; }
        public string? DealerImgPath { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OpenningBalance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalInvoice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReturnInvoice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCreditInvoice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPaidInvoice { get; set; }
    }
}