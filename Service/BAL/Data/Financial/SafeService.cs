using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class SafeService : BaseOrgService<SafeModelView, Safe>
    {
        public SafeService(string Schema) : base(Schema) { }
    }
}