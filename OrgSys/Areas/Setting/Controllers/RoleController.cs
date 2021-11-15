using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using Service;

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
                var us = new UserService(User.GetSchema()).Get(User.GetUserId());
                if (us != null)
                    us.SignIn(HttpContext , User.GetSchema());
            }
            return res;
        }
    }
}