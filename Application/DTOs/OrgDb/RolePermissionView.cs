using Domain.Entities;

namespace Application.DTOs
{
    public class RolePermissionModelView : RolePermission
    {
        public string RoleName { get; set; }

        public string PermissionName { get; set; }
    }
}