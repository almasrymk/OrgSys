using System;
using Service;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class PurchasesController : BaseReportController
    {
        public IActionResult SuppliersStatment(DateTime? FromDate = null, DateTime? ToDate = null,
           long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 2), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            if (FromDate == null)
                FromDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (ToDate == null)
                ToDate = new DateTime(DateTime.Now.Year, 12, 31);

            var obList = new SalesReportService(User.GetSchema()).GetDealersStatment(2, FromDate.Value, ToDate.Value, DealerId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsStatment", obList) : View(obList);
        }
    }
}