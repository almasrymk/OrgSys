using System.ComponentModel.DataAnnotations;

namespace Entity.ModelReport
{
    public class ProductBalance
    {
        [Key]
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal OpenningBalance { get; set; }
        public decimal Balance { get; set; }
    }
}