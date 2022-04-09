using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class SalesController : Controller
    {
        public SalesController ()
        {

        }

        public IActionResult ClientsStatment()
        {
            return View();
        }
    }
}
