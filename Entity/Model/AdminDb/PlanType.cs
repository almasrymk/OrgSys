using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("PlanType", Schema = "admin")]
    public class PlanType : BaseModel
    {
        [Required]
        public string Name { get; set; }          
    }
}