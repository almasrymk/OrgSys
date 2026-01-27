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
    using Application.Commands.Org.Setting.Account.Commands;

    [Area("Setting")]
    public class AccountController(IConfiguration configuration, IMapper mapper) : MainController<AccountModelView, CreateAccountCommand, UpdateAccountCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(AccountModelView model)
        {
            ViewBag.BranchList = new SelectList(await GetListApi<BranchModelView>(), "Id", "Name", model.ParentId);
            ViewBag.AccountType = new SelectList(await GetListApi<AccountTypeModelView>(), "Id", "Name", model.AccountTypeId);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<AccountModelView>();
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