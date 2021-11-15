using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("RolePermission")]
    public class RolePermission : BaseModel
    {
        [ForeignKey("Role")]
        public long RoleId { get; set; }

        [ForeignKey("Permission")]
        public long PermissionId { get; set; }

        public Role Role { get; set; }

        public Permission Permission { get; set; }
    }
}