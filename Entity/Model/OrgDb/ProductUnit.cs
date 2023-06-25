using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("ProductUnit")]
    public class ProductUnit : BaseModel
    {
        [ForeignKey("Product")]
        public virtual long ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Unit")]
        public virtual long UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Rate { get; set; }

        public virtual bool DefaultUnit { get; set; }
    }
}