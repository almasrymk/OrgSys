using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralPropertyElement", Schema = "admin")]
    public class GeneralPropertyElement : BaseModel
    {
        [ForeignKey("GeneralProperty")]
        public virtual long GeneralPropertyId { get; set; }

        public virtual GeneralProperty GeneralProperty { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}