using Entity.ModelView;
using Entity.Model;

namespace Service
{
    public class ClientPlanService : BaseAdminService<ClientPlanModelView, ClientPlan>
    {
        public ClientPlanService() : base("Client,Plan") { }       
    }
}