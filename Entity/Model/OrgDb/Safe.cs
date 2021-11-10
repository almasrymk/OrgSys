using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Safe", Schema = "org")]
    public class Safe : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}