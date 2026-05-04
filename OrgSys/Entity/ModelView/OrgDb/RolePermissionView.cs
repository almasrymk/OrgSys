using Entity.Model;

namespace Entity.ModelView
{
    public class RolePermissionModelView : RolePermission
    {
        public string RoleName { get; set; }

        public string PermissionName { get; set; }
    }
}