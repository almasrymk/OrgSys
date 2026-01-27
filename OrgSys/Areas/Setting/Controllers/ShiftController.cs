namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Shift.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System.Collections.Generic;
    using System.Linq;

    [Area("Setting")]
    public class ShiftController(IConfiguration configuration, IMapper mapper) : MainController<ShiftModelView, CreateShiftCommand, UpdateShiftCommand>(configuration, mapper)
    {
        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new ShiftService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
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