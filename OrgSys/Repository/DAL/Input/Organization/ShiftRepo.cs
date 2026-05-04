using Entity.Model;

namespace Repository
{
    public class ShiftRepo : CurdOrg<Shift>
    {
        public ShiftRepo(string Schema) : base(Schema) { }
    }
}