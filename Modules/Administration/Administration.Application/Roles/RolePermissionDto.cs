
namespace Administration.Application
{
    public class RolePermissionDto : RolePermission
    {
        public string? RoleName { get; set; }

        public string? PermissionName { get; set; }
    }
}