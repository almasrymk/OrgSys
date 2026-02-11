namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Account.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using iTextSharp.text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class AccountController(IConfiguration configuration, IMapper mapper) : MainController<AccountModelView, CreateAccountCommand, UpdateAccountCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(AccountModelView model)
        {
            ViewBag.AccountType = new SelectList(await GetListApi<AccountTypeModelView>(Page: 1, PageSize: 20), "Id", "Name", model.AccountTypeId);
            ViewBag.AccountList = new SelectList(await GetListApi<AccountModelView>( Page: 1, PageSize: 20), "Id", "Name", model.ParentId);
        }

        public override async Task<AccountModelView> InitializeData(AccountModelView ob)
        {
            ViewBag.AccountList = new SelectList(await GetListApi<AccountModelView>(Page: 1, PageSize: 20), "Id", "Name", ob.ParentId);
            if (ob == null)
                ob = new AccountModelView();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<AccountModelView>($"GetMax")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            ob.ParentName = (await GetObApi<AccountModelView>($"GetById?Id={ob.ParentId}"))?.Name;
            return ob;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<AccountModelView>(TextSearch:txtSearch , Page: page, PageSize: pageSize);
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