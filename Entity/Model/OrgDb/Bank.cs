using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Bank")]
    public class Bank : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }      
    }
}