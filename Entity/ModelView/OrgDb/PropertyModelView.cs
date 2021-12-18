using Utility;
using System.Linq;
using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class PropertyModelView : Property
    {
        public List<PropertyElementModelView> PropertyElementList { get; set; }
    }
}