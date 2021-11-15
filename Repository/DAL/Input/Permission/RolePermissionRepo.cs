using Entity.Model;

namespace Repository
{
    public class RolePermissionRepo : CurdOrg<RolePermission>
    {
        public RolePermissionRepo(string Schema) : base(Schema) { }
    }
}