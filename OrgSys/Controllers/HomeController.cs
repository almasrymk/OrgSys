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
        UserService user = new UserService();
        
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

        public IActionResult Notfound()
        {
            return View();
        }

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
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RequestModelView _request)
        {           
            _request.URL = "WWW.Ex@.Email.com";
            _request.ExpireDate = DateTime.Now.AddDays(2);
            if (ModelState.IsValid)
            {
                new RequestService().Save(_request);
                
            }

            return RedirectToAction("RegDone");

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
        public IActionResult LogIn(UserModelView _user)
        {
            try
            {
                var us = user.Get(_user.UserName);
                if (!string.IsNullOrEmpty(_user.NewPassword))
                {
                    us.Password = Utility.Security.Encrypt(_user.NewPassword);
                    user.Save(us);
                }
                us.SignIn(HttpContext , _user.KeepLoggedIn);
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
        public IActionResult LogInToPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogInToPass(UserModelView _user)
        {
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