using Service;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class LookupsController : BaseReportController
    {       
        public IActionResult Clients(string search, int page = 1 , int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            var obList = new LookupsReportService(User.GetSchema()).GetDealers(1, search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsList", obList) : View(obList);
        }

        public IActionResult Suppliers(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 2;
            var obList = new LookupsReportService(User.GetSchema()).GetDealers(2, search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsList", obList) : View(obList);
        }
    }
}