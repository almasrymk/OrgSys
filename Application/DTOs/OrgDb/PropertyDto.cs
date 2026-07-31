using Domain.Entities;

namespace Application.DTOs
{
    public class PropertyDto : Property
    {
        public List<PropertyElementDto>? PropertyElementList { get; set; }
    }
}