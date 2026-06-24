using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("PlanType", Schema = "admin")]
    public class PlanType : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }          
    }
}