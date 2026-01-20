using Application.Commands.Org.Setting.Account.Commands;
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
    public class AccountController(IConfiguration configuration, IMapper mapper) : MainController<AccountModelView , CreateAccountCommand , UpdateAccountCommand>(configuration , mapper)
    {
        public override async Task LoadViewBag(AccountModelView model)
        {
            ViewBag.BranchList = new SelectList(await GetListApi<AccountModelView>(), "Id", "Name", model.ParentId);
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