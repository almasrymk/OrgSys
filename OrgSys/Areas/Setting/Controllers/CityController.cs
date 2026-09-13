namespace OrgSys.Areas.Setting.Controllers
{
    using AutoMapper;
    using System.Linq;
    using OrgSys.Controllers;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MasterData.Application.Cities.Commands;

    [Area("Setting")]
    public class CityController(IConfiguration configuration, IMapper mapper) : MainController<CityDto, CreateCityCommand, UpdateCityCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(CityDto model)
        {
            ViewBag.CountryList = new SelectList(await GetListApi<CountryDto>(Page: 1, PageSize: 20), "Id", "Name", model.CountryId);
        }

        public async Task<JsonResult> GetCitiesByCountryId(int CountryId)
        {
            var cities = await GetListApi<CityDto>($"GetListByCountryId?CountryId={CountryId}&Page=1&PageSize=20");
            var cityList = cities.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(cityList);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<CityDto>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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