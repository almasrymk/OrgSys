using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Entity.Model;
using Entity.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrgSys.Models;
using Repository;
using Repository.Seed;
using Service;
using Service.BAL.Data.Security;
using Utility;

namespace OrgSys.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        LoginUserService _loginUserService;
        UserService _userService;
        DbContextOptions<OrgContext> _option;
        ClientService _clientService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _option = new DbContextOptions<OrgContext>();

            if (_loginUserService == null)
                _loginUserService = new LoginUserService();

            if (_clientService == null)
                _clientService = new ClientService();

            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                new InitialData(User.GetSchema()).Run().Wait();
                if (_userService == null)
                    _userService = new UserService(User.GetSchema());
            }
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Tables()
        {
            return View();
        }
        public IActionResult PaymentMethod()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View("Dashboard");
        }

        [AllowAnonymous]
        public IActionResult Notfound()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult InvoicePrint()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult MailTemplate()
        {
            return View(new MailViewModel { Sender = "Orgnizer", Receiver = "Ahmed Ali", Date = DateTime.Now.ToString("dd/MMM/yyyy") });
        }

        [AllowAnonymous]
        public IActionResult ServerError()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Maintenance()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-gb", culture)),
                new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return Json(culture);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegDone()
        {
            return View();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn(string ReturnUrl)
        {
            return View();
        }
       
        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogIn(LoginUserModelView _user , string ReturnUrl)
        {
            try
            {
                var us = _loginUserService.GetLoginUserName(_user.UserName);
                if (!string.IsNullOrEmpty(_user.NewPassword))
                {
                    us.Password = Utility.Security.Encrypt(_user.NewPassword);
                    _loginUserService.Save(us);
                }

                OrgContext _orgContext = new OrgContext(_option, us.Schema);
                new InitialData(us.Schema).Run().Wait();

                _userService = new UserService(us.Schema);
                var usSys = _userService.GetByLoginUserId(us.Id);
                usSys.SignIn(HttpContext, us.Schema, _user.KeepLoggedIn);
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet]
        public IActionResult LogOut()
        {
            var us = _loginUserService.Get(User.GetUserName());
            us.SignOut(HttpContext);
            return RedirectToAction("LogIn");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RequestReg()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RequestReg(RequestModelView _request)
        {
            var Key = string.Format("{0:000000000}", new Random().Next(0, 999999999));
            _request.Key = Key;
            _request.URL = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/Home/Register?Key=" + Utility.Security.Encrypt(Key);
            var LoginURL = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/Home/Login";
            var BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var SupploerURL = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/Home/Login";
            _request.ExpireDate = DateTime.Now.AddDays(2);

            var old = new RequestService().GetEmail(_request.Email);
            if (old != null && old.Id > 0)
                new RequestService().Delete(old.Id);

            new RequestService().Save(_request);

            var Body = General.RenderViewAsync<MailViewModel>(this, "MailTemplate", new MailViewModel { Date = DateTime.Now.ToString("dd MMM yyyy"), Sender = "Organizer", Receiver = _request.Name, LoginUrl = LoginURL, TechnicalSupportUrl = SupploerURL, Url = _request.URL, BaseUrl = BaseUrl }).Result;
            Utility.General.SendEmail(_request.Email, "Organizer", "Wellcom", Body);
            ViewBag.Name = _request.Name;
            ViewBag.Email = _request.Email;
            return RedirectToAction("RegDone");
        }

        public static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", string.Empty); sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string Key)
        {
            Key = Key.Replace(" ", "+");
            var keyNumber = Utility.Security.Decrypt(Key);
            var request = new RequestService().GetByKey(keyNumber);
            if (request == null || request.Id == 0 || request.ExpireDate < DateTime.Now)
                return RedirectToAction("Notfound");

            var client = new ClientService().GetRequstId(request.Id);
            if (client != null && client.Id > 0)
                return RedirectToAction("Notfound");

            if (client == null)
                client = new ClientModelView();

            client.Name = request.Name;
            client.CompanyName = request.CompanyName;
            client.Email = request.Email;
            client.Mobile = request.Phone;
            client.CodeNumber = new ClientService().GetMaxCode();
            client.Code = "" + client.CodeNumber;
            ViewBag.TypeActivityId = new SelectList(new TypeActivityService().GetAll(0, 0, 1, 10000), "Id", "Name");
            ViewBag.NationalityId = new SelectList(new NationalityService().GetAll(0, 0, 1, 10000), "Id", "Name");
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Value = "1", Text = Utility.Resource.Title_Designer.From + " 1 " + Utility.Resource.Title_Designer.To + " 5 " + Utility.Resource.Title_Designer.Employees });
            items.Add(new SelectListItem { Value = "2", Text = Utility.Resource.Title_Designer.From + " 5 " + Utility.Resource.Title_Designer.To + " 50 " + Utility.Resource.Title_Designer.Employees });
            items.Add(new SelectListItem { Value = "3", Text = Utility.Resource.Title_Designer.From + " 50 " + Utility.Resource.Title_Designer.To + " 150 " + Utility.Resource.Title_Designer.Employees });
            items.Add(new SelectListItem { Value = "4", Text = Utility.Resource.Title_Designer.From + " 150 " + Utility.Resource.Title_Designer.To + " 1500 " + Utility.Resource.Title_Designer.Employees });
            items.Add(new SelectListItem { Value = "5", Text = Utility.Resource.Title_Designer.MoreThen + " 1500 " + Utility.Resource.Title_Designer.Employees });
            ViewBag.SizeOfCompany = new SelectList(items, "Value", "Text");
            return View(client);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(ClientModelView _client, string Password)
        {
            if (ModelState.IsValid)
            {
                _client.DbSchema = _client.Email.Replace("@", "").Replace(".", "").ToUpper();
                var client = new ClientService().Save(_client);
                if (client.Id > 0)
                {
                    var loginUser = new LoginUserModelView();
                    loginUser.ClientId = client.Id;
                    loginUser.UserName = client.Email;
                    loginUser.Password = Password;
                    if (loginUser != null)
                        loginUser = new LoginUserService().Save(loginUser);

                    OrgContext _orgContext = new OrgContext(_option, client.DbSchema);
                    _orgContext.Database.EnsureCreated();
                    RelationalDatabaseCreator creator = (RelationalDatabaseCreator)_orgContext.Database.GetService<IRelationalDatabaseCreator>();
                    creator.CreateTables();
                    string createEFMigrationsHistoryCommand = $@"
                        USE [{_orgContext.Database.GetDbConnection().Database}];
                        SET ANSI_NULLS ON;
                        SET QUOTED_IDENTIFIER ON;
                        CREATE TABLE [{client.DbSchema}].[__MigrationsHistory](
                        [MigrationId] [nvarchar](150) NOT NULL,
                        [ProductVersion] [nvarchar](32) NOT NULL,
                        CONSTRAINT [PK__MigrationsHistory] PRIMARY KEY CLUSTERED 
                        (
                        [MigrationId] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                        ) ON [PRIMARY];
                        ";
                    _orgContext.Database.ExecuteSqlRaw(createEFMigrationsHistoryCommand);
                    _orgContext.Database.ExecuteSqlRaw($"INSERT INTO [{client.DbSchema}].[__MigrationsHistory](MigrationId,ProductVersion) SELECT MigrationId,ProductVersion FROM org.__MigrationsHistory");
                    new InitialData(client.DbSchema).Run().Wait();
                    var usSys = new UserModelView { BranchId = 1, RoleId = 1, Code = "1", CodeNumber = 1, UserName = loginUser.UserName, LoginUserId = loginUser.Id, Name = client.Name };
                    _userService = new UserService(client.DbSchema);
                    usSys = _userService.Save(usSys);

                    // Save Company Profile from Client Data

                    usSys.SignIn(HttpContext, client.DbSchema);
                    return RedirectToAction("Dashboard");
                }
            }
            return View(_client);

        }
        //--------------- Check For Client -------------------
        [AllowAnonymous]
        public ActionResult CheckEmailToClient(string Email)
        {
            return Json(_clientService.CheckEmailToClient(Email));
        }

        [AllowAnonymous]
        public ActionResult CheckPhoneToClient(string Phone)
        {
            return Json(_clientService.CheckPhoneToClient(Phone));
        }


        //--------------- Check For User -------------------
        [AllowAnonymous]
        public ActionResult CheckEmail(string Email)
        {
            return Json(_loginUserService.CheckEmail(Email));
        }

        [AllowAnonymous]
        public ActionResult HavePassword(string Email)
        {
            return Json(_loginUserService.HavePassword(Email));
        }

        [AllowAnonymous]
        public ActionResult CheckPassword(string Email, string Password)
        {
            return Json(_loginUserService.CheckEmailAndPassword(Email, Utility.Security.Encrypt(Password)));
        }

        public ActionResult CheckCurrentPassword(long Id, string CurrentPassword)
        {
            return Json(_loginUserService.CheckCurrentPassword(Id, Utility.Security.Encrypt(CurrentPassword)));
        }

        [HttpGet]
        public ActionResult Profile(int id, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();

            var IdUser = new UserService(User.GetSchema()).Get(id);
            return View("Profile", IdUser);
        }

        [HttpPost]
        public ActionResult Profile(UserModelView _profile)
        {
            try
            {
                _profile.RoleId = User.GetRoleId();
                _profile.ImgPath = SaveFile(_profile.ImgPath);

                if (_profile.NewPassword != null)
                    _profile.Password = _profile.NewPassword;
                else

                    _userService.Save(_profile);

                return Json(data: new { status = "success", id = _profile.Id, url = "/Home/Profile?id=" + _profile.Id + "&status=" + ResultStatus.success + "&MsgError=Success" });
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







        public async Task<IActionResult> Print()
        {
            //List<string> Css = new List<string>();
            //Css.Add("/css/main.css");
            //Css.Add("/font/iconsmind-s/css/iconsminds.css");
            //Css.Add("/font/simple-line-icons/css/simple-line-icons.css");
            //Css.Add("/css/vendor/bootstrap.min.css");
            //Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
            //Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
            //Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
            //Css.Add("/css/vendor/select2.min.css");
            //Css.Add("/css/vendor/select2-bootstrap.min.css");
            //Css.Add("/css/vendor/perfect-scrollbar.css");
            //Css.Add("/css/vendor/glide.core.min.css");
            //Css.Add("/css/vendor/bootstrap-stars.css");
            //Css.Add("/css/vendor/nouislider.min.css");
            //Css.Add("/css/vendor/bootstrap-datepicker3.min.css");
            //Css.Add("/css/vendor/component-custom-switch.min.css");
            //Css.Add("/css/vendor/bootstrap-float-label.min.css");
            //Css.Add("/css/vendor/smart_wizard.min.css");
            //Css.Add("/css/vendor/bootstrap-tagsinput.css");
            //Css.Add("/font-awesome/css/all.css");
            //Css.Add("/lib/main.css");
            //Css.Add("/alertify.js/alertify.core.css");
            //Css.Add("/alertify.js/alertify.default.css");
            //Css = Css.Select(c => {
            //    string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
            //    return output;
            //}).ToList();
            //reviewer.CssFiles = Css;

            //if ("" + reviewer.ImageBase64String == "")
            //{
            //    if (System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
            //    {
            //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(_hostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
            //        if (res != "")
            //            reviewer.ImageBase64String = "data:image/png;base64," + res;
            //    }
            //}

            //reviewer.Lookups = await GetLookups(reviewer);


            var viewHtml = await RenderViewAsync<InvoiceModelView>(this, "InvoicePrint", null);
            await Main(viewHtml, 0);
            var cd = new System.Net.Mime.ContentDisposition
            {
                //Open In New Tap Or Download
                Inline = true
            };
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
            var stream = new FileStream("PrintOut/0.pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        async Task Main(string body, int id)
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
            var ImagePath = System.IO.File.ReadAllText("wwwroot/logos/logos22.svg");
            await page.PdfAsync(path + id + ".pdf", new PuppeteerSharp.PdfOptions
            {
                Format = PuppeteerSharp.Media.PaperFormat.A4,
                PrintBackground = false,
                OmitBackground = true,
                DisplayHeaderFooter = true,
                FooterTemplate = "<div style=\"font-size: 8px; padding-top: 8px; text-align: center; width: 100%; \"><span class=\"pageNumber\"></span></div>",
                HeaderTemplate = "<div style=\"text-align:center!important; margin-top:-25px; margin-left:50%; transform: translateX(-50%);\">" + ImagePath + "</div>",
                MarginOptions = new PuppeteerSharp.Media.MarginOptions
                {
                    Bottom = "90px",
                    Top = "120px",
                    Left = "10px",
                    Right = "10px"
                }
            });
        }

        public async Task<string> RenderViewAsync<TModel>(Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

    }
}