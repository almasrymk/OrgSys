using System.Collections.Generic;
using System.Linq;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class UserController : BaseController<UserModelView>
    {
        public override void LoadViewBag(UserModelView model)
        {
            ViewBag.BranchList = new SelectList(new BranchService().GetAll(model.ParentId, model.TypeId), "Id", "Name", model.BranchId);
            ViewBag.RoleList = new SelectList(new RoleService().GetAll(model.ParentId, model.TypeId), "Id", "Name", model.RoleId);
        }

        public JsonResult CheckUDoublicat(string userName , int id)
        {
            return Json(new UserService().CheckDoublicat(userName , id));
        }

        public override ActionResult Save(UserModelView model)
        {
            var res = base.Save(model);
            if(User.IsCurrentUserAndRole(model.Id , model.RoleId))
            {
                var us = new UserService().Get(model.Id);
                if (us != null)
                    us.SignIn(HttpContext);
            }
            return res;
        }

        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new UserService().GetAll(txtSearch, 0, 0, page, pageSize);
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