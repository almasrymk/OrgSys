namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.User.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class UserController(IConfiguration configuration, IMapper mapper) : MainController<UserModelView, CreateUserCommand, UpdateUserCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(UserModelView model)
        {
            ViewBag.BranchList = new SelectList(await GetListApi<BranchModelView>(), "Id", "Name", model.BranchId);
            ViewBag.RoleList = new SelectList(await GetListApi<RoleModelView>(), "Id", "Name", model.RoleId);
        }

        //public JsonResult CheckUDoublicat(string userName, int id)
        //{
        //    return Json(new UserService(User.GetSchema()).CheckDoublicat(userName, id));
        //}

        public override async Task<ActionResult> Save(UserModelView model)
        {
            var res = await base.Save(model);
            if (User.IsCurrentUserAndRole(model.Id, model.RoleId))
            {
                var us = new UserService(User.GetSchema()).Get(model.Id);
                if (us != null)
                    us.SignIn(HttpContext, User.GetSchema());
            }
            return res;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<UserModelView>();
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