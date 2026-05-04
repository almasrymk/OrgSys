using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("PropertyElement")]
    public class PropertyElement : BaseModel
    {
        [ForeignKey("Property")]
        public virtual long PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}