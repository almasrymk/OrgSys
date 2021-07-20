using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using Service.BAL;
using Utility;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class RoleController : BaseController<RoleModelView>
    {
        public override ActionResult Save(RoleModelView model)
        {
            var res = base.Save(model);
            if (User.IsCurrentUserAndRole(User.GetUserId(), model.Id))
            {
                var us = new UserService().Get(User.GetUserId());
                if (us != null)
                    us.SignIn(HttpContext);
            }
            return res;
        }
    }
}
