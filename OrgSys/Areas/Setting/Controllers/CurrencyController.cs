using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using Service;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class CurrencyController : BaseController<CurrencyModelView>
    {
        public JsonResult GetRate(int id)
        {
            var ob = new CurrencyService(User.GetSchema()).Get(id);
            var data = new
            {
                rate = ob.Rate
            };
            return Json(data);
        }
    }
}