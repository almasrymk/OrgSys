using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class StoreService : BaseOrgService<StoreModelView, Store>
    {
        public StoreService(string Schema) : base(Schema , "Branch") { }
    }
}