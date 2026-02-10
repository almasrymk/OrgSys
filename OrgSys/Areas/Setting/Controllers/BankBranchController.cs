namespace OrgSys.Areas.Setting.Controllers
{
    using AutoMapper;
    using System.Linq;
    using Entity.ModelView;
    using OrgSys.Controllers;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using Application.Commands.Org.Setting.BankBranch.Commands;

    [Area("Setting")]
    public class BankBranchController(IConfiguration configuration, IMapper mapper) : MainController<BankBranchModelView, CreateBankBranchCommand, UpdateBankBranchCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(BankBranchModelView model)
        {
            ViewBag.BankList = new SelectList(await GetListApi<BankModelView>(), "Id", "Name", model.BankId);
            ViewBag.CountryList = new SelectList(await GetListApi<CountryModelView>(), "Id", "Name", model.CountryId);
            ViewBag.CityList = new SelectList(await GetListApi<CityModelView>(), "Id", "Name", model.CityId);
            ViewBag.DistrictList = new SelectList(await GetListApi<DistrictModelView>(), "Id", "Name", model.DistrictId);
        }

        public async Task<JsonResult> GetCitiesByCountryId(int countryId)
        {
            var cities = await GetListApi<CityModelView>();
            var cityList = cities.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(cityList);
        }

        public async Task<JsonResult> GetDistrictsByCityId(int cityId)
        {
            var districts = await GetListApi<DistrictModelView>();
            var districtsiList = districts.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(districtsiList);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            var itemsList = await GetListApi<BankModelView>();
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