using Entity.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class UserModelView : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual long RoleId { get; set; }
 
        public virtual long? BranchId { get; set; }
         
        public virtual long LoginUserId { get; set; }

        public string RoleName { get; set; }

        public string BranchName { get; set; }

        public bool KeepLoggedIn { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string NewPassword { get; set; }
       
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}