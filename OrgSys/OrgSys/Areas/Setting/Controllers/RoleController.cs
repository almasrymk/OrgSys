namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Role.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class RoleController(IConfiguration configuration, IMapper mapper) : MainController<RoleModelView, CreateRoleCommand, UpdateRoleCommand>(configuration, mapper)
    {
        public override async Task<ActionResult> Save(RoleModelView model)
        {
            var res = await base.Save(model);
            if (User.IsCurrentUserAndRole(User.GetUserId(), model.Id))
            {
                var us = new UserService(User.GetSchema()).Get(User.GetUserId());
                if (us != null)
                    us.SignIn(HttpContext, User.GetSchema());
            }
            return res;
        }
    }
}