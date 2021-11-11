using Entity.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    [Table("LoginUser", Schema = "admin")]
    public class LoginUserModelView : BaseModel
    {
        public LoginUserModelView()
        {

        }

        public LoginUserModelView(LoginUser ob)
        {
            if (ob == null)
                ob = new LoginUser();

            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.MaskText = ob.MaskText;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.Hide = ob.Hide;
            this.ImgPath = ob.ImgPath;
            this.Status = ob.Status;
            this.UserName = ob.UserName;
            this.Password = Utility.Security.Decrypt(ob.Password);
        }

        public LoginUser Model()
        {
            return new LoginUser
            {
                Id = this.Id,
                CodeNumber = this.CodeNumber,
                Code = this.Code,
                MaskText = this.MaskText,
                ParentId = this.ParentId,
                TypeId = this.TypeId,
                Hide = this.Hide,
                ImgPath = this.ImgPath,
                Status = this.Status,
                UserName = this.UserName,
                Password = Utility.Security.Encrypt(this.Password)
            };
        }

        [Required]
        public string UserName { get; set; }
       
        public string Password { get; set; }
       
        public long ClientId { get; set; }
    }
}