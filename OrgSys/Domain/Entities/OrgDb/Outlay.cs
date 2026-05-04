using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Outlay")]
    public class Outlay : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}