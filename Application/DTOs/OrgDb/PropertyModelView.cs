using Domain.Entities;

namespace Application.DTOs
{
    public class PropertyModelView : Property
    {
        public List<PropertyElementModelView> PropertyElementList { get; set; }
    }
}