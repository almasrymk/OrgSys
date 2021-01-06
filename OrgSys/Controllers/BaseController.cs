using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service;
using Utility;

namespace OrgSys.Controllers
{
    public class BaseController<entity> : Controller where entity : BaseModel
    {
        string AreaName = "";
        string ControllerName = "";
        BaseService<entity> service;
        [HttpGet]
        public virtual ActionResult Index(string search, int page = 1, int pageSize = 20, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();
            ViewBag.pageNumber = page;
            LoadViewBagIndex();
            var list = service.GetAll(search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("List", list) : View("Index", list);
        }

        [HttpGet]
        public virtual ActionResult Save(long id = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
        {
            var ob = service.Get(id);
            if (ob == null || ob.Id == 0)
                ob = (entity)Activator.CreateInstance(typeof(entity));
            LoadViewBag(ob);
            return View(ob);
        }

        [HttpPost]
        public virtual ActionResult Save(entity model)
        {
            if (ModelState.IsValid)
            {
                model = service.Save(model);
                return Redirect("/" + AreaName + "/" + ControllerName + "?status=" + ResultStatus.success + "&MsgError=Success");
            }
            LoadViewBag(model);
            return View(model);
        }

        [HttpGet]
        public virtual JsonResult Delete(long id)
        {
            var ob = service.Get(id);
            try
            {
                if (ob != null && ob.Id > 0)
                {
                    service.Delete(id);
                    return Json("Ok");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json("Error");
        }

        [HttpPost]
        public virtual JsonResult DeleteList(long[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    service.Delete(ids.ToList());
                    return Json("Ok");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message); 
            }
            return Json("Error");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AreaName = "" + context.RouteData.Values["area"];
            ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            Assembly assembly = Assembly.Load("Service");
            service = (BaseService<entity>)assembly.CreateInstance("Service.BAL." + RouteData.Values["controller"] + "Service");
            ViewBag.Page = "/" + context.RouteData.Values["area"] + "/" + ControllerContext.ActionDescriptor.ControllerName;
            ViewBag.area = AreaName;
            ViewBag.PageTitle = ControllerContext.ActionDescriptor.ControllerName;
            base.OnActionExecuting(context);
        }

        public virtual void LoadViewBag(entity model)
        {

        }

        public virtual void LoadViewBagIndex()
        {

        }
    }
}
