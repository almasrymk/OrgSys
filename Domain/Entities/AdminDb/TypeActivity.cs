using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TypeActivity", Schema = "admin")]
    public class TypeActivity : BaseEntity
    {
        [Required]
        public virtual string Name { get; set; }       
    }
}