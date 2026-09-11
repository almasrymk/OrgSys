namespace OrgSys.Areas.Setting.Controllers
{
    using Administration.Application.Roles.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    
    using System.Threading.Tasks;

    [Area("Setting")]
    public class RoleController(IConfiguration configuration, IMapper mapper) : MainController<RoleDto, CreateRoleCommand, UpdateRoleCommand>(configuration, mapper)
    {
        public override async Task<ActionResult> Save(RoleDto model)
        {
            var res = await base.Save(model);
            if (User.IsCurrentUserAndRole(User.GetUserId(), model.Id))
            {
                //var us = new UserService(User.GetSchema()).Get(User.GetUserId());
                //if (us != null)
                //    us.SignIn(HttpContext, User.GetSchema());
            }
            return res;
        }
    }
}