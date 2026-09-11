using System.ComponentModel.DataAnnotations;

namespace Administration.Application
{
    public class RoleDto : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public  string? Name { get; set; }

        public List<RolePermission>? PermissionList { get; set; }

        public List<TreeView>? PermissionsTree { get; set; }
    }
}