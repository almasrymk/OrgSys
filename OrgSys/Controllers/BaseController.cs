using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public virtual ActionResult Index(string search, long ParentId = 0, long TypeId = 0, int page = 1, int pageSize = 28, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();
            ViewBag.pageNumber = page;
            ViewBag.ParentId = ParentId;
            ViewBag.TypeId = TypeId;
            LoadViewBagIndex(ParentId , TypeId);
            var list = service.GetAll(search, ParentId, TypeId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("List", list) : View("Index", list);
        }

        [HttpGet]
        public virtual ActionResult Save(long id = 0, long ParentId = 0, long TypeId = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
        {
            var ob = service.Get(id);
            if (ob == null || ob.Id == 0)
            {
                if (ob == null)
                    ob = (entity)Activator.CreateInstance(typeof(entity));
                ob.ParentId = ParentId;
                ob.TypeId = TypeId;
            }
            ob = InitializeData(ob);
            LoadViewBag(ob);
            return View(ob);
        }

        [HttpPost]
        public virtual ActionResult Save(entity model)
        {
            if (ModelState.IsValid)
            {
                model.ImgPath = SaveFile(model.ImgPath);
                model = service.Save(model);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(data: new { status = "success" , id = model.Id , url = "/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success" });
                return Redirect("/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success");
            }
            LoadViewBag(model);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json("Error");
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
                    DeleteFile(ob.ImgPath);
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
        public virtual JsonResult DeleteList(long[] ids , long ParentId = 0 , long TypeId = 0)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    var list = service.GetAll(ids.ToList(), TypeId);
                    var res = service.Delete(ids.ToList());
                    if (res)
                        DeleteFile(list.Select(e => e.ImgPath).ToList());
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

        public virtual void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {

        }

        public virtual entity InitializeData(entity ob)
        {
            return ob;
        }

        [HttpPost]
        public virtual JsonResult SaveFile(int id)
        {
            try
            {
                var ob = service.Get(id);
                if (ob == null || ob.Id == 0)
                    return Json("error");
                var LastPath = ob.ImgPath;
                string NewPath = null;
                string path = Path.GetFullPath("~/wwwroot").Replace("~\\", "");
                string oldPath = Path.GetFullPath("~/wwwroot" + LastPath).Replace("~\\", "").Replace(@"\\", @"\");
                if ("" + LastPath != "" && System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);

                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        if ("" + formFile.FileName != "")
                        {
                            NewPath = "/Files/" + ControllerContext.ActionDescriptor.ControllerName + string.Format("{0:000000000}", new Random().Next(999999999)) + Path.GetExtension(formFile.FileName);
                            using (var inputStream = new FileStream(path + NewPath, FileMode.Create))
                            {
                                // read file to stream
                                formFile.CopyTo(inputStream);
                                // stream to byte array
                                byte[] array = new byte[inputStream.Length];
                                inputStream.Seek(0, SeekOrigin.Begin);
                                inputStream.Read(array, 0, array.Length);
                                // get file name
                                string fName = formFile.FileName;
                            }
                        }
                    }
                }

                ob.ImgPath = NewPath;
                service.Save(ob);
                return Json("Ok");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public virtual string SaveFile(string LastPath)
        {
            string NewPath = null;
            string path = Path.GetFullPath("~/wwwroot").Replace("~\\", "");
            string oldPath = Path.GetFullPath("~/wwwroot" + LastPath).Replace("~\\", "").Replace(@"\\", @"\");
            if ("" + LastPath != "" && System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);

            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length > 0)
                {
                    if ("" + formFile.FileName != "")
                    {
                        NewPath = "/Files/" + ControllerContext.ActionDescriptor.ControllerName + string.Format("{0:000000000}", new Random().Next(999999999)) + Path.GetExtension(formFile.FileName);
                        using (var inputStream = new FileStream(path + NewPath, FileMode.Create))
                        {
                            // read file to stream
                            formFile.CopyTo(inputStream);
                            // stream to byte array
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                            // get file name
                            string fName = formFile.FileName;
                        }
                    }
                }
            }
            return NewPath;
        }

        public virtual bool DeleteFile(string LastPath)
        {
            string oldPath = Path.GetFullPath("~/wwwroot" + LastPath).Replace("~\\", "").Replace(@"\\", @"\");
            if ("" + LastPath != "" && System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
            return true;
        }

        public virtual bool DeleteFile(List<string> LastPaths)
        {
            foreach (var LastPath in LastPaths)
            {
                string oldPath = Path.GetFullPath("~/wwwroot" + LastPath).Replace("~\\", "").Replace(@"\\", @"\");
                if ("" + LastPath != "" && System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            return true;
        }
    }
}
