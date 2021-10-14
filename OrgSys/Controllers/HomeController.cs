using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity.Model;
using Entity.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrgSys.Models;
using Service;

namespace OrgSys.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {

            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Index()
        {
            try
            {
                var us = new UserService().Get(2);
                if (us != null)
                    us.SignIn(HttpContext);
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("Dashboard");
            // return View();
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

        UserService user = new UserService();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserModelView _user)
        {

            if (ModelState.IsValid)
            {
                var check = user.Get(_user.UserName);
                if (check == null)
                {
                    user.Save(_user);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.error = "Email or UserName already exists";
                    return View();
                }

            }

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
            var us = user.Get(_user.UserName);
           
            if (us != null)
            {
                if (string.IsNullOrEmpty(us.Password))
                {
                    us.Password = Utility.Security.Encrypt(_user.NewPassword);
                    user.Save(us);
                }
                else
                {
                  if(!user.CheckEmailAndPassword(_user.UserName, Utility.Security.Encrypt(_user.Password)))
                    {                        
                        return View(_user);
                    }
                }
                us.SignIn(HttpContext);
                return RedirectToAction("Dashboard");
            }

            return View("Register");
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

        public ActionResult CheckEmail(string Email)
        {
            return Json(user.CheckEmail(Email));
        }

        public ActionResult HavePassword(string Email)
        {
            return Json(user.HavePassword(Email));
        }

        public ActionResult CheckPassword(string Email , string Password)
        {
            return Json(user.CheckEmailAndPassword(Email, Utility.Security.Encrypt(Password)));
        }
    }
}