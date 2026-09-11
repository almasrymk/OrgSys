namespace OrgSys.Areas.Setting.Controllers
{
    using Treasury.Application.BankBranches.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class BankBranchController(IConfiguration configuration, IMapper mapper) : MainController<BankBranchDto, CreateBankBranchCommand, UpdateBankBranchCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(BankBranchDto model)
        {
            ViewBag.BankList = new SelectList(await GetListApi<BankDto>( Page: 1, PageSize: 20), "Id", "Name", model.BankId);
            ViewBag.CountryList = new SelectList(await GetListApi<CountryDto>(Page: 1, PageSize: 20), "Id", "Name", model.CountryId);
            ViewBag.CityList = new SelectList(await GetListApi<CityDto>($"GetListByCountryId?CountryId={model.CountryId}&Page=1&PageSize=20"), "Id", "Name", model.CityId);
            ViewBag.DistrictList = new SelectList(await GetListApi<DistrictDto>($"GetListByCityId?CityId={model.CityId}&Page=1&PageSize=20"), "Id", "Name", model.DistrictId);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", long bankId = 0, int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            var itemsList = await GetListApi<BankBranchDto>(ParentId: bankId, TextSearch: txtSearch, Page: page, PageSize: pageSize);
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