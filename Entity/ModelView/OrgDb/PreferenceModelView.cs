using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class PreferenceModelView : Preference
    {

        public IEnumerable<PreferenceModelView> PreferenceList { get; set; } = new List<PreferenceModelView>(); 
    }
}