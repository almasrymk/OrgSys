using Entity.Model;

namespace Repository
{
    public class RoleRepo : CurdOrg<Role>
    {
        public RoleRepo(string Schema) : base(Schema) { }
    }
}