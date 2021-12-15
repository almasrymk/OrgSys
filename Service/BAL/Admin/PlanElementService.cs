using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class PlanElementService : BaseAdminService<PlanElementModelView, PlanElement>
    {
        public PlanElementService() : base("Plan") { }
    }
}