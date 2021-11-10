using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Unit", Schema = "org")]
    public class Unit : BaseModel
    {
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
    }
}