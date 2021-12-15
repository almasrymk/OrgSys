using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Classification")]
    public class Classification : BaseModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public bool BePurchased { get; set; }

        public bool BeSold { get; set; }

        public bool BeManufactured { get; set; }
    }
}