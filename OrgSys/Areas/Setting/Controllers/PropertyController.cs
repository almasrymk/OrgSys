using System.Collections.Generic;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class PropertyController : BaseController<PropertyModelView>
    {
        public override PropertyModelView InitializeData(PropertyModelView ob)
        {
            if (ob.PropertyElementList == null)
                ob.PropertyElementList = new List<PropertyElementModelView>();
            return ob;
        }
    }
}