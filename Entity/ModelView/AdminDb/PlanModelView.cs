using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class PlanModelView : Plan
    {
        public string PlanTypeName { get; set; }
        public List<PlanElementModelView> PlanElementList { get; set; }
    }
}