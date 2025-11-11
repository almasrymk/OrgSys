using Application.Commands.Org.City.Create;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class CityController(ISender sender) : BaseController<CityModelView>
    {
        public override void LoadViewBag(CityModelView model)
        {
            ViewBag.BranchList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
        }

        public JsonResult GetList(string txtSearch = "", int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new CityService(User.GetSchema()).GetAll(txtSearch, 0, 0, page, pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name
                })
                .ToList();
            return Json(list);
        }

        public override ActionResult Save(CityModelView model)
        {
            var res = sender.Send(new CreateCommand(model.CountryId, model.Name), new CancellationToken());
            return View(res);
        }
        //public override async Task<ActionResult> Save(CityModelView model)
        //{
        //    var res = sender.Send(new CreateCommand(model.CountryId, model.Name), new CancellationToken());
        //    return View(res);
        //    //return base.Save(model);
        //}
    }
}