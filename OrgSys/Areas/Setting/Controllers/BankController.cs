namespace OrgSys.Areas.Setting.Controllers
{
    using Treasury.Application.Banks.Commands;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]

    public class BankController(IConfiguration configuration, IMapper mapper) : MainController<BankDto, CreateBankCommand, UpdateBankCommand>(configuration, mapper)
    {
        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<BankDto>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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