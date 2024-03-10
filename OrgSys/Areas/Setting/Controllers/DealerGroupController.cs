using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using Service;

using System.Linq;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class DealerGroupController :  BaseController<DealerGroupModelView>
    {
        public override DealerGroupModelView InitializeData(DealerGroupModelView ob)
        {
            if (ob == null)
                ob = new DealerGroupModelView();
            if (ob.Id == 0)
            {
                ob.CodeNumber = new DealerGroupService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
            }
            return ob;
        }

        public JsonResult GetList(string txtSearch = "", long TypeId = 0, int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            long TypeDealerId = TypeId == 1 || TypeId == 3 ? 1 : 2;

            var itemsList = new DealerGroupService(User.GetSchema()).GetAll(txtSearch, 0, TypeDealerId, page, pageSize);
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
