using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class PlanService : BaseAdminService<PlanModelView, Plan>
    {
        public PlanService() : base("PlanType,PlanElements") { }
    }
}