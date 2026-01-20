using Application.Commands.Org.Setting.City.Commands;
using Application.Commands.Org.Setting.Country.Commands;
using AutoMapper;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrgSys.Controllers;
using Service;
using System.Linq;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class CountryController(IConfiguration configuration, IMapper mapper) :MainController<CountryModelView, CreateCountryCommand, UpdateCountryCommand>(configuration , mapper)
    {
        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new BranchService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
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
