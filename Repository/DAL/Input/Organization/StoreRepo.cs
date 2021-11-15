using Entity.Model;

namespace Repository
{
    public class StoreRepo : CurdOrg<Store>
    {
        public StoreRepo(string Schema) : base(Schema) { }
    }
}