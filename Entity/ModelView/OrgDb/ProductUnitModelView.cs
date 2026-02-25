using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class ProductUnitModelView : BaseModel
    { 
        public virtual long ProductId { get; set; }

        public virtual long UnitId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public  decimal Rate { get; set; }

        public  bool DefaultUnit { get; set; }

        public virtual string ProductName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public virtual string UnitName { get; set; }
    }
}