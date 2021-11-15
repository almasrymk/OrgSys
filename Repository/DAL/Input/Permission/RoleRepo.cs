using Entity.Model;

namespace Repository
{
    public class RolechRepo : CurdOrg<Role>
    {
        public RolechRepo(string Schema) : base(Schema) { }
    }
}