using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralPropertyElement", Schema = "admin")]
    public class GeneralPropertyElement : BaseModel
    {
        [ForeignKey("GeneralProperty")]
        public long GeneralPropertyId { get; set; }

        public GeneralProperty GeneralProperty { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}