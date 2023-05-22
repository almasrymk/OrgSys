using System.ComponentModel.DataAnnotations;

namespace Entity.ModelReport
{
    public class ProductBalance
    {
        [Key]
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        //public decimal OpenningBalance { get; set; }
        public decimal Balance { get; set; }
        public string ProductImgPath { get; set; }
        //public string ClassificationImgPath { get; set; }
        public long ClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public long StockId { get; set; }
        public string StockName { get; set; }

    }
}