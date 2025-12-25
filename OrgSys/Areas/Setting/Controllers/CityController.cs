using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OrgSys.Controllers;
using Service;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class CityController(IConfiguration configuration) : MainController<CityModelView>(configuration)
    {               
        public override async Task LoadViewBag(CityModelView model)
        {
            ViewBag.BranchList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
        }

        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new CityService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
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