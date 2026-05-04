using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("PlanElement", Schema = "admin")]
    public class PlanElement : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual long PlanId { get; set; }
        public virtual Plan Plan { get; set; }
    }
}