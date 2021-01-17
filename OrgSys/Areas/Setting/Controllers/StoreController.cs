using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service.BAL;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class StoreController : BaseController<StoreModelView>
    {
        public override void LoadViewBag(StoreModelView model)
        {
            ViewBag.BranchId = new SelectList(new BranchService().GetAll(model.ParentId, model.TypeId), "Id", "Name", model.BranchId);
        }

    }
}
