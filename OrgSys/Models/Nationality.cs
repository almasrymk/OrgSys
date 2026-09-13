using System.ComponentModel.DataAnnotations;

namespace OrgSys.Models
{
    public class Nationality : BaseModel
    {
        [Required]
        public virtual string Name { get; set; } = null!;
    }
}
