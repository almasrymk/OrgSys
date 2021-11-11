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
using Utility;

namespace OrgSys.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        LoginUserService user = new LoginUserService();

        public AccountController(ILogger<AccountController> logger)
        {

            _logger = logger;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginUserModelView _user)
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
        public IActionResult LogIn(LoginUserModelView _user)
        {
            var us = user.GetLoginUserName(_user.UserName);

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
                var usSys = new UserService().GetByLoginUserId(us.Id);
                usSys.SignIn(HttpContext , _user.KeepLoggedIn);
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
            if (Email == null)
                Email = "";
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

        public ActionResult CheckCurrentPassword(long Id, string CurrentPassword)
        {
            return Json(user.CheckCurrentPassword(Id,Utility.Security.Encrypt(CurrentPassword)));
        }


        //------------------------ Profile Action ------------------------
        [HttpGet]
        public ActionResult Profile(int id, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();

            var IdUser = user.Get(id);
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
                    //_profile.Password 
                new UserService().Save(_profile);
                return Json(data: new { status = "success", id = _profile.Id, url = "/Account/Profile?id=" + _profile.Id + "&status=" + ResultStatus.success + "&MsgError=Success" });
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


    }
}
