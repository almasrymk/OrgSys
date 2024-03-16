using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
using System.Linq;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class BankBranchController : BaseController<BankBranchModelView>
    {
        public override void LoadViewBag(BankBranchModelView model)
        {
            ViewBag.BankList = new SelectList(new BankService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.BankId);
            ViewBag.CountryList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
            // ViewBag.CityList = new SelectList(new CityService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CityId);
        }

        public JsonResult GetCitiesByCountryId(int countryId)
        {
            var cities = new CityService(User.GetSchema()).GetById(countryId);
            var cityList = cities.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(cityList);
        }

        public JsonResult GetDistrictsByCityId(int cityId)
        {
            var districts = new DistrictService(User.GetSchema()).GetById(cityId);
            var districtsiList = districts.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(districtsiList);
        }

        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new BankBranchService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
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
