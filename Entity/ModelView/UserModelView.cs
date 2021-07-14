using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.UserName = ob.UserName;
            this.Password = ob.Password;
            this.BranchId = ob.BranchId;
            this.BranchName = ob.Branch?.Name;
            this.RoleId = ob.RoleId;
            this.RoleName = ob.Role?.Name;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public User Model
        {
            get
            {
                return new User
                {
                    Id = this.Id,
                    Name = this.Name,
                    UserName = this.UserName,
                    Password = this.Password,
                    BranchId = this.BranchId,
                    RoleId = this.RoleId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }

        [Display(Name = nameof(Title_Designer.UserName), ResourceType = typeof(Title_Designer))]
        [StringLength(15, MinimumLength = 5)]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.UsernameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string UserName { get; set; }

        [Display(Name = nameof(Title_Designer.Password), ResourceType = typeof(Title_Designer))]               
        public string Password { get; set; }

        [Display(Name = nameof(Title_Designer.Role), ResourceType = typeof(Title_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.RoleRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public long RoleId { get; set; }

        [Display(Name = nameof(Title_Designer.Role), ResourceType = typeof(Title_Designer))]
        public string RoleName { get; set; }

        [Display(Name = nameof(Title_Designer.Branch), ResourceType = typeof(Title_Designer))]
        public long? BranchId { get; set; }

        [Display(Name = nameof(Title_Designer.Branch), ResourceType = typeof(Title_Designer))]
        public string BranchName { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}