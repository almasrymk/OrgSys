using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Report
{
    public class ProductCost
    {
        public long Id { get; set; }
        public long ClassificationId { get; set; }
        public string? ClassificationName { get; set; }
        public long ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AvgCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FirstCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastCost { get; set; }
    }
}