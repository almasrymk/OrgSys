using Administration.Domain;

namespace Administration.Application
{
    public class PreferenceDto : Preference
    {

        public IEnumerable<PreferenceDto>? PreferenceList { get; set; } = new List<PreferenceDto>(); 
    }
}