using Entity.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class UserModelView : BaseModel
    {
        public UserModelView()
        {

        }

        public UserModelView(User ob)
        {
            if (ob == null)
                ob = new User();

            this.Name = ob.Name;

            this.UserName = ob.UserName;

            this.Password =  ob.Password;

            this.BranchId = ob.BranchId;

            this.BranchName = ob.Branch?.Name;

            this.RoleId = ob.RoleId;

            this.RoleName = ob.Role?.Name;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;
        }

        public User Model()
        {
            return new User
            {
                Name = this.Name,
                UserName = this.UserName,
                Password = this.Password,
                BranchId = this.BranchId,
                RoleId = this.RoleId,
                Id = this.Id,
                CodeNumber = this.CodeNumber,
                Code = this.Code,
                MaskText = this.MaskText,
                ParentId = this.ParentId,
                TypeId = this.TypeId,
                Hide = this.Hide,
                Status = this.Status,
                ImgPath = this.ImgPath,
                
            };
        }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public long RoleId { get; set; }

        public string RoleName { get; set; }

        public long? BranchId { get; set; } 

        public string BranchName { get; set; }
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string NewPassword { get; set; }
       
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}