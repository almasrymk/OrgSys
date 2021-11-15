using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("ProductUnit")]
    public class ProductUnit : BaseModel
    {
        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey("Unit")]
        public long UnitId { get; set; }

        public Unit Unit { get; set; }

        [Required]
        public decimal Rate { get; set; }

        public bool DefaultUnit { get; set; }
    }
}