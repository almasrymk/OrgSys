using System;
using System.Diagnostics;
using Entity.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrgSys.Models;
using Service;
using Service.BAL.Data.Security;

namespace OrgSys.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        LoginUserService user = new LoginUserService();
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult PaymentMethod()
        {
            return View();
        }

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
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }       

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LogIn(LoginUserModelView _user)
        {
            try
            {
                var us = user.Get(_user.UserName);
                if (!string.IsNullOrEmpty(_user.NewPassword))
                {
                    us.Password = Utility.Security.Encrypt(_user.NewPassword);
                    user.Save(us);
                }
                var usSys  = new UserService().GetByLoginUserId(us.Id);
                usSys.SignIn(HttpContext , _user.KeepLoggedIn);
                Utility.General.HttpContext = HttpContext;
                Utility.General.SetSchema(us.Schema);
                return RedirectToAction("Dashboard");
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            var us = user.Get(User.GetUserName());
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
            _request.ExpireDate = DateTime.Now.AddDays(2);

            var old = new RequestService().GetEmail(_request.Email);
            if(old != null && old.Id > 0)           
                new RequestService().Delete(old.Id);

            new RequestService().Save(_request);
            Utility.General.SendEmail(_request.Email, "Organizer", "Wellcom", _request.URL);
            return RedirectToAction("RegDone");
        }
      
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string Key)
        {
            Key = Key.Replace(" ", "+");
            var ky = Utility.Security.Decrypt(Key);
            var rq = new RequestService().GetByKey(ky);
            if (rq == null || rq.Id == 0 || rq.ExpireDate < DateTime.Now)
                return RedirectToAction("Notfound");

            
            return View();
        }

        [AllowAnonymous]
        public ActionResult CheckEmail(string Email)
        {
            return Json(user.CheckEmail(Email));
        }

        [AllowAnonymous]
        public ActionResult HavePassword(string Email)
        {
            return Json(user.HavePassword(Email));
        }

        [AllowAnonymous]
        public ActionResult CheckPassword(string Email, string Password)
        {
            return Json(user.CheckEmailAndPassword(Email, Utility.Security.Encrypt(Password)));
        }
    }
}