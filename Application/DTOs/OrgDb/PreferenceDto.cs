using Domain.Entities;

namespace Application.DTOs
{
    public class PreferenceDto : Preference
    {

        public IEnumerable<PreferenceDto> PreferenceList { get; set; } = new List<PreferenceDto>(); 
    }
}