using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("RolePermission")]
    public class RolePermission : BaseModel
    {
        [ForeignKey("Role")]
        public virtual long RoleId { get; set; }

        [ForeignKey("Permission")]
        public virtual long PermissionId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Permission Permission { get; set; }
    }
}