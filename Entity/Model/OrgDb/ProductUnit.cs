using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("ProductUnit", Schema = "org")]
    public class ProductUnit : BaseModel
    {        
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        [Required]
        public decimal Rate { get; set; }

        public bool DefaultUnit { get; set; }
    }
}