using Application.Commands.Org.City.Create;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Utility;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class CityController(ISender sender) : Controller
    {
        string AreaName = "";
        string ControllerName = "";

        [HttpGet]
        public virtual ActionResult Index(string search, long ParentId = 0, long TypeId = 0, int page = 1, int pageSize = 10, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();
            ViewBag.pageNumber = page;
            ViewBag.ParentId = ParentId;
            ViewBag.TypeId = TypeId;
            //LoadViewBagIndex(ParentId, TypeId);
            var list = new Service.CityService("org").GetAll(search, ParentId, TypeId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("List", list) : View("Index", list);
        }

        public void LoadViewBag(CityModelView model)
        {
            ViewBag.BranchList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
        }

        //public override void LoadViewBag(CityModelView model)
        //{
        //    ViewBag.BranchList = new SelectList(new CountryService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.CountryId);
        //}

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

        [HttpGet]
        public virtual ActionResult Save(long id = 0, long ParentId = 0, long TypeId = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
        {
            ViewBag.TypeId = TypeId;
            var ob = new Service.CityService("org").Get(id);
            if (ob == null || ob.Id == 0)
            {
                if (ob == null)
                    ob = new CityModelView();
                ob.ParentId = ParentId;
                ob.TypeId = TypeId;
            }
            //ob = InitializeData(ob);
            LoadViewBag(ob);
            return View(ob);
        }

        [HttpPost]
        public async Task<ActionResult> Save(CityModelView model)
        {
            var res = await sender.Send(new CreateCommand(model.CountryId, model.Name), HttpContext.RequestAborted);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Ok(new { status = "success" , id = model.Id , url = "/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success" });
                return Redirect("/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        //public override async Task<ActionResult> Save(CityModelView model)
        //{
        //    var res = sender.Send(new CreateCommand(model.CountryId, model.Name), new CancellationToken());
        //    return View(res);
        //    //return base.Save(model);
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AreaName = "" + context.RouteData.Values["area"];
            ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            Assembly assembly = Assembly.Load("Service");
            var ServiceName = "Service." + ControllerName + "Service";
            var type = assembly.GetType(ServiceName);
            //service = (IBaseService<entity>)Activator.CreateInstance(type, User.GetSchema());
            ViewBag.Page = "/" + context.RouteData.Values["area"] + "/" + ControllerContext.ActionDescriptor.ControllerName;
            ViewBag.area = AreaName;
            ViewBag.PageTitle = ControllerContext.ActionDescriptor.ControllerName;
            base.OnActionExecuting(context);
        }        
    }
}