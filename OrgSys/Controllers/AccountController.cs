using Entity.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        UserService user = new UserService();

        public AccountController(ILogger<AccountController> logger)
        {

            _logger = logger;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Requests()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

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
                    return RedirectToAction("Dashboard", "Home");
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
                    if (!user.CheckEmailAndPassword(_user.UserName, Utility.Security.Encrypt(_user.Password)))
                    {
                        return View(_user);
                    }
                }
                us.SignIn(HttpContext);
                return RedirectToAction("Dashboard","Home");
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


        public ActionResult CheckPassword(string Email, string Password)
        {
            return Json(user.CheckEmailAndPassword(Email, Utility.Security.Encrypt(Password)));
        }


        //------------------------ Profile Action ------------------------
        [HttpGet]
        public ActionResult Profile(int id)
        {
            var IdUser = user.Get(id);
            return View("Profile", IdUser);
        }

        [HttpPost]
        public ActionResult Profile(UserModelView _profile)
        {
            _profile.RoleId = User.GetRoleId();

            var IsExsit = user.CheckEmailAndPassword(_profile.UserName, Utility.Security.Encrypt(_profile.Password));
            if (IsExsit)
            {
                _profile.Password = Utility.Security.Encrypt(_profile.NewPassword);
                _profile.ImgPath = SaveFile(_profile.ImgPath);
                new UserService().Save(_profile);
            }

             return View(_profile);
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


    }
}
