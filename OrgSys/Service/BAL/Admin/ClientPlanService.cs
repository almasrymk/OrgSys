using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class ClientPlanService : BaseAdminService<ClientPlanModelView, ClientPlan>
    {
        public ClientPlanService() : base("Client,Plan") { }       
    }
}