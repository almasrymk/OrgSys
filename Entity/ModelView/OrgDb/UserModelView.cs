using Entity.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class UserModelView : User
    { 
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