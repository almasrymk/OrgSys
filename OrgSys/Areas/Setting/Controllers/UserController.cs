namespace OrgSys.Areas.Setting.Controllers
{
    using Administration.Application.Users.Commands;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class UserController(IConfiguration configuration, IMapper mapper) : MainController<UserDto, CreateUserCommand, UpdateUserCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(UserDto model)
        {
            ViewBag.BranchList = new SelectList(await GetListApi<BranchDto>(Page: 1, PageSize: 20), "Id", "Name", model.BranchId);
            ViewBag.RoleList = new SelectList(await GetListApi<RoleDto>(Page: 1, PageSize: 20), "Id", "Name", model.RoleId);
        } 

        public override async Task<ActionResult> Save(UserDto model)
        {
            var res = await base.Save(model);
            if (User.IsCurrentUserAndRole(model.Id, model.RoleId))
            {
                var us = (await GetObApi<UserDto>($"GetById?Id={model.Id}"));
                if (us != null)
                    us.SignIn(HttpContext, User.GetSchema());
            }
            return res;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<UserDto>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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