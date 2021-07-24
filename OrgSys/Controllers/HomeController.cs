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
            return View();
        }
      
        public IActionResult Index()
        {
            return View();
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
        public IActionResult Register()
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
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
