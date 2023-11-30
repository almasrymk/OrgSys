using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Property")]
   public class Property : BaseModel
    {       
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual ICollection<PropertyElement> PropertyElements { get; set; }
    }
}