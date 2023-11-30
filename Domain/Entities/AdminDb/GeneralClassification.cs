using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralClassification", Schema = "admin")]
    public class GeneralClassification : BaseModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual bool BePurchased { get; set; }

        public virtual bool BeSold { get; set; }

        public virtual bool BeManufactured { get; set; }
    }
}