using System.ComponentModel.DataAnnotations;

namespace OrgSys.Models
{
    public class LoginUser : BaseModel
    {
        [Required]
        public virtual string UserName { get; set; } = null!;

        [Required]
        public virtual string Password { get; set; } = null!;

        public virtual long ClientId { get; set; }

        public virtual Client? Client { get; set; }
    }
}
