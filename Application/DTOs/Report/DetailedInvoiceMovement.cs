using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Report
{
    public class DetailedInvoiceMovement
    {
        public long Id { get; set; }
        public long InvTypeId { get; set; }
        public string InvCode { get; set; }
        public string  InvTypeName { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public DateTime Date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Paid { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
        public long BranchId { get; set; }
        public string  BranchName { get; set; }
        public long ShiftId { get; set; }
        public string ShiftName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}