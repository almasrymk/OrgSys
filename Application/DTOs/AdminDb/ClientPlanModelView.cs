using Domain.Entities;

namespace Application.DTOs
{
    public class ClientPlanModelView : ClientPlan
    {       
        public string ClientName { get; set; }

        public string PlanName { get; set; }
    }
}