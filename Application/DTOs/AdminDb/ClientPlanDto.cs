using Domain.Entities;

namespace Application.DTOs
{
    public class ClientPlanDto : ClientPlan
    {       
        public string ClientName { get; set; }

        public string PlanName { get; set; }
    }
}