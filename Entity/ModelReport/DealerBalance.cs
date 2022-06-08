using System.ComponentModel.DataAnnotations;

namespace Entity.ModelReport
{
    public class DealerBalance
    {
        [Key]
        public long DealerId { get; set; }

        public string DealerName { get; set; }

        public decimal Amount { get; set; }      
    }
}