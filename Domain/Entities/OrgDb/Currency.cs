using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Currency")]
    public class Currency : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Rate { get; set; }

        public virtual bool IsDefault { get; set; }
    }
}