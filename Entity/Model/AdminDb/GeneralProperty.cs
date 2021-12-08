using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralProperty", Schema = "admin")]
   public class GeneralProperty : BaseModel
    {       
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<GeneralPropertyElement> GeneralPropertyElements { get; set; }
    }
}