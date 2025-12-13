using Application.Commands.Org.City;
using Application.Commands.Org.City.Commands;
using Application.Commands.Org.City.Queries;
using Entity.ModelView;
using iTextSharp.text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class CityController(ISender sender) : Controller
    {
        string AreaName = "";
        string ControllerName = "";

        [HttpGet]
        public async Task<ActionResult> Index(string search, long ParentId = 0, long TypeId = 0, int page = 1, int pageSize = 10, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();
            ViewBag.pageNumber = page;
            ViewBag.ParentId = ParentId;
            ViewBag.TypeId = TypeId;
            //LoadViewBagIndex(ParentId, TypeId); 
            var list = await sender.Send(new SearchQuery(search, ParentId, TypeId, page, pageSize), HttpContext.RequestAborted);
            //var list = new Service.CityService("org").GetAll(search, ParentId, TypeId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("List", list) : View("Index", list);
        }

        public void LoadViewBag(CityModelView model)
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

        [HttpGet]
        public async Task<ActionResult> Save(long id = 0, long ParentId = 0, long TypeId = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
        {
            ViewBag.ParentId = ParentId;
            ViewBag.TypeId = TypeId;
            var ob = new CityModelView { ParentId = ParentId, TypeId = TypeId };
            var res = await sender.Send(new GetByIdQuery(id), HttpContext.RequestAborted);
            if (res != null && res.Response != null && res.Response.Id > 0)
                ob = res.Response;

            //ob = InitializeData(ob);
            LoadViewBag(ob);
            return View(ob);
        }

        [HttpPost]
        public async Task<ActionResult> Save(CityModelView model)
        {
            var res = await sender.Send(
               model.Id == 0 ? new CreateCommand(model.CountryId, model.Name) : new UpdateCommand(model.Id, model.CountryId, model.Name)
                , HttpContext.RequestAborted);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Ok(new { status = "success", id = model.Id, url = "/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success" });
            return Redirect("/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        [HttpGet]
        public async Task<Result> Delete(long id)
        {
            var ob = await sender.Send(new GetByIdQuery(id), HttpContext.RequestAborted);
            try
            {
                if (ob != null && ob.Response.Id > 0)
                {
                    //DeleteFile(ob.ImgPath);
                    await sender.Send(new DeleteCommand(ob.Response.Id), HttpContext.RequestAborted);
                    //service.Delete(id);
                    return new Result();
                }
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, ex.Message);
            }
            return new Result(HttpStatusCode.BadRequest, "Error");
        }

        [HttpPost]
        public async Task<Result> DeleteList(long[] ids, long ParentId = 0, long TypeId = 0)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {                    
                    var res = await sender.Send(new DeleteListCommand(ids.ToList()), HttpContext.RequestAborted);
                    //if (res)
                    //DeleteFile(list.Select(e => e.ImgPath).ToList());
                    return new Result();
                }
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, ex.Message);
            }
            return new Result(HttpStatusCode.BadRequest, "Error");
        }

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