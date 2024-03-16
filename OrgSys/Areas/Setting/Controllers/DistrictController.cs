using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
using System.Linq;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class DistrictController : BaseController<DistrictModelView>
    {
        public override void LoadViewBag(DistrictModelView model)
        {
            ViewBag.CountryList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
           // ViewBag.CityList = new SelectList(new CityService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CityId);
        }

        public JsonResult GetCitiesByCountryId(int countryId)
        {
            var cities = new CityService(User.GetSchema()).GetById(countryId);
            var cityList = cities.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(cityList);
        }
        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new DistrictService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
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
