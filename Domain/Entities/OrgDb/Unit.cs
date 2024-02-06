using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Unit")]
    public class Unit : BaseModel
    {
        [StringLength(50, MinimumLength = 2)]
        public virtual string? Name { get; set; }
    }
}