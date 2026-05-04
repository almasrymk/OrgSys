using Entity.Model;

namespace Repository
{
    public class OutlayRepo : CurdOrg<Outlay>
    {
        public OutlayRepo(string Schema) : base(Schema) { }
    }
}