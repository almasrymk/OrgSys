using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {        
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}