using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using Service.BAL;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class DealerController : BaseController<DealerModelView>
    {
        public override DealerModelView InitializeData(DealerModelView ob)
        {
            if (ob == null)
                ob = new DealerModelView();
            if (ob.Id == 0)
            {
                ob.CodeNumber = new DealerService().GetMaxCode(ob.TypeId);
                ob.Code = "" + new DealerService().GetMaxCode(ob.TypeId);
            }
            return ob;
        }

        public JsonResult GetList(string txtSearch = "", long TypeId = 0, int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            long TypeDealerId = TypeId == 1 || TypeId == 3 ? 1 : 2;

            var itemsList = new DealerService().GetAll(txtSearch , 0 , TypeDealerId, page , pageSize);
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