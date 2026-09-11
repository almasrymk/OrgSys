using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reporting.Application
{
    public class ProductTransaction
    {
        public long Id { get; set; }
        public long ClassificationId { get; set; }
        public string? ClassificationName { get; set; }
        public long ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public DateTime Date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        public long BranchId { get; set; }
        public string? BranchName { get; set; }
        public long ShiftId { get; set; }
        public string? ShiftName { get; set; }
        public long UserId { get; set; }
        public string? UserName { get; set; }
    }
}