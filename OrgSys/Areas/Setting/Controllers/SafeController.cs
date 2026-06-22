namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Safe.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class SafeController(IConfiguration configuration, IMapper mapper) : MainController<SafeModelView, CreateSafeCommand, UpdateSafeCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(SafeModelView model)
        {
            ViewBag.AccountList = new SelectList(await GetListApi<AccountModelView>(TypeId: model.TypeId, Page: 1, PageSize: 20), "Id", "Name", model.AccountId);
        }

        public override async Task<SafeModelView> InitializeData(SafeModelView ob)
        {
            ViewBag.AccountList = new SelectList(await GetListApi<AccountModelView>(TypeId: ob.TypeId, Page: 1, PageSize: 20), "Id", "Name", ob.AccountId);

            if (ob == null)
                ob = new SafeModelView();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<SafeModelView>($"GetMax?TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            ob.AccountName = (await GetObApi<AccountModelView>($"GetById?Id={ob.AccountId ?? 0}"))?.Name;

            return ob;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<SafeModelView>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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