using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralPropertyDto : GeneralProperty
    {
        public List<GeneralPropertyElementDto> GeneralPropertyElementList { get; set; }
    }
}