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

        public string ProductName { get; set; }

        public string UnitName { get; set; }
    }
}