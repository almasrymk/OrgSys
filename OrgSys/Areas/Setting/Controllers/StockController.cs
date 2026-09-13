namespace OrgSys.Areas.Setting.Controllers
{
    using global::Inventory.Application.Stocks.Commands;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class StockController(IConfiguration configuration, IMapper mapper) : MainController<StockDto, CreateStockCommand, UpdateStockCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(StockDto model)
        {
            ViewBag.BranchList = new SelectList(await GetListApi<BranchDto>(Page: 1, PageSize: 20), "Id", "Name", model.BranchId);
        }

        public override async Task<StockDto> InitializeData(StockDto ob)
        {
            ob.AccountName = (await GetObApi<AccountDto>($"GetById?Id={ob.AccountId ?? 0}"))?.Name;
            return ob;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<StockDto>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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
