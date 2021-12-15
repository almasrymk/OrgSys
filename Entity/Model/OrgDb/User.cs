using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("User")]
    public class User : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }
       
        public string Password { get; set; }

        [ForeignKey("Role")]
        public long RoleId { get; set; }

        public virtual Role Role { get; set; }

        [ForeignKey("Branch")]
        public long? BranchId { get; set; }

        public virtual Branch Branch { get; set; }

        public long LoginUserId { get; set; }
    }
}