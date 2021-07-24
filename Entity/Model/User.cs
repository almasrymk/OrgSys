using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("User")]
    public class User : BaseModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string UserName { get; set; }
       
        public string Password { get; set; }

        [Required]
        public long RoleId { get; set; }

        public virtual Role Role { get; set; }
 
        public long? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
    }
}