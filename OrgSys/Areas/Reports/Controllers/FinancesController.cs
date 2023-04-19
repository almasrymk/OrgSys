using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrgSys.Areas.Reports.Controllers
{
    public class FinancesController : Controller
    {
        public IActionResult SafeMovement()
        {
            return View();
        }

        public IActionResult SafeBalance()
        {
            return View();
        }

        public IActionResult TotalsPerPeriod()
        {
            return View();
        }
    }
}
