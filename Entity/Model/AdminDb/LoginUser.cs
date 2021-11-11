using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("LoginUser", Schema = "admin")]
    public class LoginUser : BaseModel
    {     
        [Required]
        public string UserName { get; set; }
       
        public string Password { get; set; }
       
        public long ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}