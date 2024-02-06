using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Model;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class FinancesController : Controller
    {
        public IActionResult SafeMovement(string FromDate = null, string ToDate = null,
            long DealerId = 0, long SafeId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 100)
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.SafeList = new SelectList(new SafeService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.DealerId = DealerId;
            ViewBag.SafeId = SafeId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;

            var d = new DealerService(User.GetSchema()).Get(DealerId);
            var s = new SafeService(User.GetSchema()).Get(SafeId);
            if (d == null || s == null)
            {
                d = new DealerModelView();
                s = new SafeModelView();

            }

            var obList = new ReportResult<SafeStatmentData, SafeStatment>
            {
                PrintMode = false,
                Data = new SafeStatmentData { SafeBalance = new SafeBalance { SafeName = d.Name, SafeId = d.Id, DealerId = s.Id, DealerName = s.Name}, FromDate = fDate, ToDate = tDate },
                Result = new FinancesReportService(User.GetSchema()).GetSafeStatment(fDate, tDate, DealerId, SafeId, ShiftId, BranchId, UserId, page, pageSize)
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SafeMovementList", obList) : View(obList);
        }

        public IActionResult SafeBalance(string ToDate = null,
           long SafeId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.SafeList = new SelectList(new SafeService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.SafeId = SafeId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.ToDate = tDate;

            //var obList = new FinancesReportService(User.GetSchema()).GetSafeBalance(1, tDate, SafeId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SafeBalanceList", null) : View(null);
        }

        //public IActionResult SafeBalance()
        //{
        //    return View();
        //}

        public IActionResult TotalsPerPeriod()
        {
            return View();
        }
    }
}
