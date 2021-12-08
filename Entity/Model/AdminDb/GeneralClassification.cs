using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralClassification", Schema = "admin")]
    public class GeneralClassification : BaseModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public bool BePurchased { get; set; }

        public bool BeSold { get; set; }

        public bool BeManufactured { get; set; }
    }
}