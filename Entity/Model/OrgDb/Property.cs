using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Property")]
   public class Property : BaseModel
    {       
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<PropertyElement> propertyElements { get; set; }
    }
}