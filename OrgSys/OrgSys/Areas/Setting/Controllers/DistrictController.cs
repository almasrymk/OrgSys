namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.District.Commands;
    using AutoMapper;
    using Entity.Model;
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
            ViewBag.CountryList = new SelectList(await GetListApi<CountryModelView>(Page: 1, PageSize: 20), "Id", "Name", model.CountryId);
            if (model.Id > 0)
                ViewBag.CityList = new SelectList(await GetListApi<CityModelView>($"GetListByCountryId?CountryId={model.CountryId}&Page=1&PageSize=20"), "Id", "Name", model.CityId);
        }

        public async Task<JsonResult> GetDistrictesByCityId(int CityId)
        {
            var districtes = await GetListApi<DistrictModelView>($"GetListByCityId?CityId={CityId}&Page=1&PageSize=20");
            var districtList = districtes.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToList();
            return Json(districtList);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<DistrictModelView>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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