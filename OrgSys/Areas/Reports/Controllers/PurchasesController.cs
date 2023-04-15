using System;
using Service;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.ModelReport;
using Entity.ModelView;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class PurchasesController : BaseReportController
    {
        public IActionResult SuppliersStatment(string FromDate = null, string ToDate = null,
           long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 2), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.DealerId = DealerId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            if (DealerId == 1)
                DealerId = 0;

            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;

            var d = new DealerService(User.GetSchema()).Get(DealerId);
            if (d == null)
                d = new DealerModelView();

            var obList = new ReportResult<DealerStatmentData, DealerStatment>
            {
                PrintMode = false,
                Data = new DealerStatmentData { DealerBalance = new DealerBalance { DealerName = d.Name, DealerImgPath = d.ImgPath, DealerId = d.Id }, FromDate = fDate, ToDate = tDate },
                Result = new SalesReportService(User.GetSchema()).GetDealersStatment(2, fDate, tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize)
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SuppliersStatmentList", obList) : View(obList);
        }

        public IActionResult SuppliersBalance(string ToDate = null, long DealerId = 0, long ShiftId = 0, 
            long BranchId = 0, long UserId = 0, int page = 1, int pageSize = 100)
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 2), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.ToDate = tDate;

            var obList = new SalesReportService(User.GetSchema()).GetDealersBalance(2, tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SuppliersBalanceList", obList) : View(obList);
        }
    }
}