using Application.Commands.Org.Setting.Account.Commands;
using Application.Commands.Org.Setting.BankBranch.Commands;
using AutoMapper;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OrgSys.Controllers;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class BankBranchController(IConfiguration configuration, IMapper mapper) : MainController<BankBranchModelView, CreateBankBranchCommand, UpdateBankBranchCommand>(configuration , mapper) 
    {
        public override async Task LoadViewBag(BankBranchModelView model)
        {
            ViewBag.BankList = new SelectList(await GetListApi<BankModelView>(), "Id", "Name", model.BankId);
            ViewBag.CountryList = new SelectList(await GetListApi<CountryModelView>(), "Id", "Name", model.CountryId);
        }

        public async Task<JsonResult> GetCitiesByCountryId(int countryId)
        {
            var cities = await GetListApi<CityModelView>($"GetListByCountryId?KeySearch=&CountryId={countryId}&Page=1&PageSize=20");
            var cityList = cities.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(cityList);
        }

        public async Task<JsonResult> GetDistrictsByCityId(int cityId)
        {
            var districts = await GetListApi<DistrictModelView>($"GetListByCityId?KeySearch=&CityId={cityId}&Page=1&PageSize=20");
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