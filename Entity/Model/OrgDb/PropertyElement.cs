using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("PropertyElement", Schema = "org")]
    public class PropertyElement : BaseModel
    {
        public long PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}