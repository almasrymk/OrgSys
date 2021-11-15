using Entity.Model;

namespace Repository
{
    public class UnitRepo : CurdOrg<Unit>
    {
        public UnitRepo(string Schema) : base(Schema) { }
    }
}