using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class DealerService : BaseOrgService<DealerModelView, Dealer>
    {
        public DealerService(string Schema) : base(Schema , "DealerGroup,Country,City,District") { }
    }
}