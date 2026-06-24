using Domain.Entities;

namespace Application.DTOs
{
    public class PlanDto : Plan
    {
        public string PlanTypeName { get; set; }
        public List<PlanElementDto> PlanElementList { get; set; }
    }
}