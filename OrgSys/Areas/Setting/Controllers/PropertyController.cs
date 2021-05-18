using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Repository;
using Service.BAL;
using Utility;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class PropertyController : BaseController<PropertyModelView>
    {
        public override PropertyModelView InitializeData(PropertyModelView ob)
        {
            if (ob.PropertyElements == null)
                ob.PropertyElements = new List<PropertyElementModelView>();
            return ob;
        }

    }
}
