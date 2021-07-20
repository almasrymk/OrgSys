using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service.BAL;
using Utility;

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

        [HttpGet]
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

    }
}
