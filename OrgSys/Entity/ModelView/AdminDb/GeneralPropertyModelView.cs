using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class GeneralPropertyModelView : GeneralProperty
    {
        public List<GeneralPropertyElementModelView> GeneralPropertyElementList { get; set; }
    }
}