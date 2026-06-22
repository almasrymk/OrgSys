using Domain.Entities;

namespace Application.DTOs
{
    public class PlanModelView : Plan
    {
        public string PlanTypeName { get; set; }
        public List<PlanElementModelView> PlanElementList { get; set; }
    }
}