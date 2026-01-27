namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.District.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class DistrictController(IConfiguration configuration, IMapper mapper) : MainController<DistrictModelView, CreateDistrictCommand, UpdateDistrictCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(DistrictModelView model)
        {
            ViewBag.CountryList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
        }

        public async Task<JsonResult> GetCitiesByCountryId(int countryId)
        {
            var cities = await GetListApi<CityModelView>($"GetCitiesByCountryId?countryId={countryId}");
            var cityList = cities.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(cityList);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<DistrictModelView>();
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