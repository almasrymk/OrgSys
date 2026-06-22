using Domain.Entities;

namespace Application.DTOs
{
    public class PreferenceModelView : Preference
    {

        public IEnumerable<PreferenceModelView> PreferenceList { get; set; } = new List<PreferenceModelView>(); 
    }
}