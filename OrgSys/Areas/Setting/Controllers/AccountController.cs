using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
using System.Linq;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class AccountController : BaseController<AccountModelView>
    {
        public override void LoadViewBag(AccountModelView model)
        {
            ViewBag.BranchList = new SelectList(new AccountService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.ParentId);
            ViewBag.AccountType = new SelectList(new AccountTypeService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.AccountTypeId);

        }

        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new AccountService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
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
