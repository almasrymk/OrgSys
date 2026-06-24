using Domain.Entities;

namespace Repository
{
    public class RoleRepo : CurdOrg<Role>
    {
        public RoleRepo(string Schema) : base(Schema) { }
    }
}