using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("PlanElement", Schema = "admin")]
    public class PlanElement : BaseEntity
    {
        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual long PlanId { get; set; }
        public virtual Plan Plan { get; set; }
    }
}