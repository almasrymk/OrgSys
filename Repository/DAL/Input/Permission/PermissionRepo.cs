using Entity.Model;

namespace Repository
{
    public class PermissionRepo : CurdOrg<Permission>
    {
        public PermissionRepo(string Schema) : base(Schema) { }
    }
}