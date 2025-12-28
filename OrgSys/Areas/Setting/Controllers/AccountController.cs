using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OrgSys.Controllers;
using Service;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class AccountController(IConfiguration configuration) : MainController<AccountModelView>(configuration)
    {
        public override async Task LoadViewBag(AccountModelView model)
        {
            ViewBag.BranchList = new SelectList(await GetListApi<AccountModelView>(Domain.Enums.ApiMethodType.Get, $"GetList?KeySearch=&Page=1&PageSize=20"), "Id", "Name", model.ParentId);
            //ViewBag.BranchList = new SelectList(new AccountService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.ParentId);
            ViewBag.AccountType = new SelectList(await GetListApi<AccountTypeModelView>(Domain.Enums.ApiMethodType.Get, $"GetList?KeySearch=&Page=1&PageSize=20"), "Id", "Name", model.AccountTypeId);
            //ViewBag.AccountType = new SelectList(new AccountTypeService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.AccountTypeId);

        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<AccountModelView>(Domain.Enums.ApiMethodType.Get, $"GetList?KeySearch={txtSearch}&Page={page}&PageSize={pageSize}");
            //var itemsList = new AccountService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
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
