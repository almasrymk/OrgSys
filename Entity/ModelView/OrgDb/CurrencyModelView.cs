using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class CurrencyModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        public bool IsDefault { get; set; }
    }
}