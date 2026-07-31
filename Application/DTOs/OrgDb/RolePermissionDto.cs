using Domain.Entities;

namespace Application.DTOs
{
    public class RolePermissionDto : RolePermission
    {
        public string? RoleName { get; set; }

        public string? PermissionName { get; set; }
    }
}