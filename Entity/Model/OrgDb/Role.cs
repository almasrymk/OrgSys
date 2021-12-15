using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Role")]
    public class Role : BaseModel
    {        
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}