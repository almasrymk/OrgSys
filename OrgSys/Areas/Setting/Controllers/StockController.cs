namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Stock.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class StockController(IConfiguration configuration, IMapper mapper) : MainController<StockModelView, CreateStockCommand, UpdateStockCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(StockModelView model)
        {
            ViewBag.BranchList = new SelectList(await GetListApi<BranchModelView>(Page: 1, PageSize: 20), "Id", "Name", model.BranchId);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<StockModelView>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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