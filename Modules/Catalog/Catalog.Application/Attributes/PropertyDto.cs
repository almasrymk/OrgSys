
namespace Catalog.Application
{
    public class PropertyDto : Property
    {
        public List<PropertyElementDto>? PropertyElementList { get; set; }
    }
}