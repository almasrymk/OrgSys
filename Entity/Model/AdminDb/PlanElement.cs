using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("PlanElement", Schema = "admin")]
    public class PlanElement : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public long PlanId { get; set; }
        public virtual Plan Plan { get; set; }
    }
}