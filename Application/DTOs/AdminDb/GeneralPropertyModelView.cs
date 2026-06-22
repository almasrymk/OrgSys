using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralPropertyModelView : GeneralProperty
    {
        public List<GeneralPropertyElementModelView> GeneralPropertyElementList { get; set; }
    }
}