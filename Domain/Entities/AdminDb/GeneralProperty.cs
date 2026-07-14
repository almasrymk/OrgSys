using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralProperty", Schema = "admin")]
   public class GeneralProperty : BaseModel
    {       
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        public virtual ICollection<GeneralPropertyElement>? GeneralPropertyElements { get; set; }
    }
}