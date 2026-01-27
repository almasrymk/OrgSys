namespace OrgSys.Areas.Setting.Controllers
{
    using Service;
    using AutoMapper;
    using System.Linq;
    using Entity.ModelView;
    using OrgSys.Controllers;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Application.Commands.Org.Setting.City.Commands;

    [Area("Setting")]
    public class CityController(IConfiguration configuration, IMapper mapper) : MainController<CityModelView, CreateCityCommand, UpdateCityCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(CityModelView model)
        {
            ViewBag.BranchList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<CityModelView>();
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