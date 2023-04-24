using Service;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Products(long ClassificationId , int page = 1, int pageSize = 100)
        {
            ViewBag.ClassificationList = new SelectList(new ClassificationService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.ClassificationId = ClassificationId;
            var obList = new LookupsReportService(User.GetSchema()).GetProducts(ClassificationId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ProductsList", obList) : View(obList);
        }

        public IActionResult Stocks(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;            
            var obList = new LookupsReportService(User.GetSchema()).GetStocks(search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StocksList", obList) : View(obList);
        }

        public IActionResult Safes(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            var obList = new LookupsReportService(User.GetSchema()).GetSafes(search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StocksList", obList) : View(obList);
        }
    }
}