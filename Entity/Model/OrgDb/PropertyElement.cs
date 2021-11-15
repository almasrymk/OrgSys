using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("PropertyElement")]
    public class PropertyElement : BaseModel
    {
        [ForeignKey("Property")]
        public long PropertyId { get; set; }

        public Property Property { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}