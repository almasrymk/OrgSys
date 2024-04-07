using Microsoft.AspNetCore.Mvc;

namespace OrgSys.Areas.Setting.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
