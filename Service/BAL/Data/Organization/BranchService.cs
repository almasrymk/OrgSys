using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class BranchService : BaseOrgService<BranchModelView, Branch>
    {
        public BranchService(string Schema) : base(Schema) { }
    }
}