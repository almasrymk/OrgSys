using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class RoleService : BaseOrgService<RoleModelView, Role>
    {
        public RoleService(string Schema) : base(Schema) { }
    }
}