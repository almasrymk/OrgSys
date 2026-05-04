using Entity.Model;

namespace Repository
{
    public class DealerRepo : CurdOrg<Dealer>
    {
        public DealerRepo(string Schema) : base(Schema) { }
    }
}