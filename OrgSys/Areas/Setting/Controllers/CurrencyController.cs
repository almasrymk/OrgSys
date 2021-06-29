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
    public class CurrencyController : BaseController<CurrencyModelView>
    {
        public JsonResult GetRate(int id)
        {
            var ob = new CurrencyService().Get(id);
            var data = new
            {
                rate = ob.Rate
            };
            return Json(data);
        }
    }
}
