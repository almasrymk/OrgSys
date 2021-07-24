using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("RolePermission")]
    public class RolePermission : BaseModel
    {
        public long RoleId { get; set; }

        public long PermissionId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Permission Permission { get; set; }
    }
}