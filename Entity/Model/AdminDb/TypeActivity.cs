using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("TypeActivity", Schema = "admin")]
    public class TypeActivity : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }       
    }
}