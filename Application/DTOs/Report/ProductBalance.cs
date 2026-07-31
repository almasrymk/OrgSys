using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Report
{
    public class ProductBalance
    {
        [Key]
        public long ProductId { get; set; }
        public string? ProductName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public string? ProductImgPath { get; set; }
        //public string ClassificationImgPath { get; set; }
        public long ClassificationId { get; set; }
        public string? ClassificationName { get; set; }
        public long StockId { get; set; }
        public string? StockName { get; set; }

    }
}