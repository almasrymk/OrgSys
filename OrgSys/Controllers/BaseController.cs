using System;
using Domain.Entities;
using Utility;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Net;
using Newtonsoft.Json;

namespace OrgSys.Controllers
{
    [Authorize]
    public class BaseController<Tentity> : Controller where Tentity : BaseModel
    {
        string AreaName = "";
        string ControllerName = "";
        IBaseService<Tentity> service;
               
        [HttpGet]
        public virtual ActionResult Index(string search, long ParentId = 0, long TypeId = 0, int page = 1, int pageSize = 10, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
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
            ViewBag.TypeId = TypeId;
            var ob = service.Get(id);
            if (ob == null || ob.Id == 0)
            {
                if (ob == null)
                    ob = (Tentity)Activator.CreateInstance(typeof(Tentity));
                ob.ParentId = ParentId;
                ob.TypeId = TypeId;
            }
            ob = InitializeData(ob);
            LoadViewBag(ob);
            return View(ob);
        }

        [HttpPost]
        public virtual ActionResult Save(Tentity model)
        {
            if (ModelState.IsValid)
            {
                model= GetUserData(model);
                model.ImgPath = SaveFile(model.ImgPath);
                model = service.Save(model);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Ok(new { status = "success" , id = model.Id , url = "/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success" });
                return Redirect("/" + AreaName + "/" + ControllerName + "?ParentId=" + model.ParentId + "&TypeId=" + model.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success");
            }
            LoadViewBag(model);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return BadRequest("Error");
            return View(model);
        }
       
        [HttpGet]
        public virtual Result Delete(long id)
        {
            var ob = service.Get(id);
            try
            {
                if (ob != null && ob.Id > 0)
                {
                    DeleteFile(ob.ImgPath);
                    service.Delete(id);
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
        public virtual Result DeleteList(long[] ids , long ParentId = 0 , long TypeId = 0)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    var list = service.GetAll(ids.ToList(), TypeId);
                    var res = service.Delete(ids.ToList());
                    if (res)
                        DeleteFile(list.Select(e => e.ImgPath).ToList());
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
            service = (IBaseService<Tentity>) Activator.CreateInstance(type, User.GetSchema());
            //service = (BaseService<entity>)assembly.CreateInstance("Service." + RouteData.Values["controller"] + "Service");
            ViewBag.Page = "/" + context.RouteData.Values["area"] + "/" + ControllerContext.ActionDescriptor.ControllerName;
            ViewBag.area = AreaName;
            ViewBag.PageTitle = ControllerContext.ActionDescriptor.ControllerName;
            base.OnActionExecuting(context);
        }

        public virtual void LoadViewBag(Tentity model)
        {

        }

        public virtual void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {

        }

        public virtual Tentity InitializeData(Tentity ob)
        {
            return ob;
        }

        [HttpPost]
        public virtual Result SaveFile(int id)
        {
            try
            {
                var ob = service.Get(id);
                if (ob == null || ob.Id == 0)
                    return new Result(HttpStatusCode.BadRequest, "Error");
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
                return new Result();
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, ex.Message);
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

        public virtual Tentity GetUserData(Tentity model)
        {
            PropertyInfo CreateUserPro = model.GetType().GetProperty("CreateUserId");
            if(CreateUserPro != null)
            {
                var userId = (long)CreateUserPro.GetValue(model);
                if(userId == 0)
                {
                    CreateUserPro.SetValue(model , User.GetUserId());
                    PropertyInfo DatePro = model.GetType().GetProperty("CreateDate");
                    if(DatePro != null)
                        DatePro.SetValue(model, DateTime.Now);
                }
            }
           
            if(model.Id > 0)
            {
                PropertyInfo ModifyUserPro = model.GetType().GetProperty("ModifyUserId");
                if (ModifyUserPro != null)
                    ModifyUserPro.SetValue(model, User.GetUserId());
                PropertyInfo DatePro = model.GetType().GetProperty("ModifyDate");
                if (DatePro != null)
                    DatePro.SetValue(model, DateTime.Now);
            }   
            return model;
        }

        public virtual async Task<IActionResult> Print(long Id , string ViewName)
        {
            List<string> Css = new List<string>();
            Css.Add("/css/vendor/bootstrap.min.css");
            Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
            Css.Add("/css/vendor/fullcalendar.min.css");
            Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
            Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
            Css.Add("/css/vendor/select2.min.css");
            Css.Add("/css/vendor/select2-bootstrap.min.css");
            Css.Add("/css/vendor/perfect-scrollbar.css");
            Css.Add("/css/vendor/glide.core.min.css");
            Css.Add("/css/vendor/bootstrap-stars.css");
            Css.Add("/css/vendor/nouislider.min.css");
            Css.Add("/css/vendor/smart_wizard.min.css");
            Css.Add("/css/vendor/component-custom-switch.min.css");
            Css.Add("/css/main.css");
            Css.Add("/css/jquery.bonsai.css");
            Css.Add("/fontawesome-free-5.15.3-web/css/all.css");
            Css.Add("/css/vendor/bootstrap-datepicker3.min.css");

            Css = Css.Select(c =>
            {
                string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
                return output;
            }).ToList();

            var ob = service.Get(Id);
            ob.CssFiles = Css;
            //if ("" + ob.ImageBase64String == "")
            //{
            //    if (System.IO.File.Exists(IHostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
            //    {
            //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(HostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
            //        if (res != "")
            //            ob.ImageBase64String = "data:image/png;base64," + res;
            //    }
            //}

            var viewHtml = await Utility.General.RenderViewAsync<Tentity>(this, ViewName, ob);
            await Main(viewHtml, 0 , 10);
            var cd = new System.Net.Mime.ContentDisposition
            {
                //Open In New Tap Or Download
                Inline = true
            };
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
            var stream = new FileStream("PrintOut/0.pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        async Task Main(string body, int id , int count)
        {
            var browserFetcher = new PuppeteerSharp.BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new PuppeteerSharp.LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(body);
            string path = @"PrintOut/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (System.IO.File.Exists(path + id + ".pdf"))
            { System.IO.File.Delete(path + id + ".pdf"); }
            var baseURL = Request.Scheme + "://" + Request.Host;
            //var ImagePath = System.IO.File.ReadAllText("wwwroot/logos/logos22.svg");
            await page.PdfAsync(path + id + ".pdf", new PuppeteerSharp.PdfOptions
            {
                Format = new PuppeteerSharp.Media.PaperFormat(decimal.Parse("2.24409"),  1 + (count/4)),// PuppeteerSharp.Media.PaperFormat.A4,
                PrintBackground = false,
                OmitBackground = true,
                DisplayHeaderFooter = true,
                //FooterTemplate = "<div style=\"font-size: 8px; padding-top: 8px; text-align: center; width: 100%; \"><span class=\"pageNumber\"></span></div>",
               // HeaderTemplate = "<div style=\"text-align:center!important; margin-top:-25px; margin-left:50%; transform: translateX(-50%);\">" + ImagePath + "</div>",
                MarginOptions = new PuppeteerSharp.Media.MarginOptions
                {
                    Bottom = "90px",
                    Top = "120px",
                    Left = "10px",
                    Right = "10px"
                }
            });
        }
    }
}