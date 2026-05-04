using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Nationality", Schema = "admin")]
    public class Nationality : BaseEntity
    {
        [Required]
        public virtual string Name { get; set; }       
    }
}