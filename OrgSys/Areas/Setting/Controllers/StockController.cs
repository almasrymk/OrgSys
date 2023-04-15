using System.Collections.Generic;
using System.Linq;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class StockController : BaseController<StockModelView>
    {
        public override void LoadViewBag(StockModelView model)
        {
            ViewBag.BranchList = new SelectList(new BranchService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.BranchId);
        }

        public JsonResult GetList(string txtSearch = "",  int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new StockService(User.GetSchema()).GetAll(txtSearch, 0,0, page, pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name
                })
                .ToList();
            return Json(list);
        }
    }
}