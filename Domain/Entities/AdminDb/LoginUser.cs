using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("LoginUser", Schema = "admin")]
    public class LoginUser : BaseEntity
    {     
        [Required]
        public virtual string UserName { get; set; }
       
        public virtual string Password { get; set; }
       
        public virtual long ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}