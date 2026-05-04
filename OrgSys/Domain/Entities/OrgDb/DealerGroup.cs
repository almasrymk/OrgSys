using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("DealerGroup")]
    public class DealerGroup : BaseEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }      
    }
}