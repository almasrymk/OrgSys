using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralProductPropertyElementDto : GeneralProductPropertyElement
    {
        public string GeneralProductName { get; set; }
         
        public string GeneralPropertyName { get; set; }

        public string GeneralPropertyElementName { get; set; }
    }
}