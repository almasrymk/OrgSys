using System.ComponentModel.DataAnnotations;

namespace OrgSys.Models
{
    public class TypeActivity : BaseModel
    {
        [Required]
        public virtual string Name { get; set; } = null!;
    }
}
