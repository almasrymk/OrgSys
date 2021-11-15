using Entity.Model;

namespace Repository
{
    public class SafeRepo : CurdOrg<Safe>
    {
        public SafeRepo(string Schema) : base(Schema) { }
    }
}