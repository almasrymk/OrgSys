using Domain.Entities;

namespace Application.DTOs
{
    public class PlanElementModelView : PlanElement
    {
        public string PlanName { get; set; }
    }
}