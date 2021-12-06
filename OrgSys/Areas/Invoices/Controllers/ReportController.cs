using Microsoft.AspNetCore.Mvc;
using Service;
using System;

namespace OrgSys.Areas.Invoices.Controllers
{
    [Area("Invoices")]
    public class ReportController : Controller
    {
        ReportService reportService;
        public ReportController()
        {
            reportService = new ReportService(User.GetSchema());
        }

        public ActionResult Clients()
        {
            return View();
        }

        public ActionResult SalesDetail(DateTime? fromDate, DateTime? toDate, long dealerId = 0, long shiftId = 0, long branchId = 0, long userId = 0, int page = 1, int pageSize = 100)
        {
            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);

            if (dealerId == 1)
                dealerId = 0;

            ViewBag.FromDate = fromDate;            
            ViewBag.ToDate = toDate; 
            ViewBag.DealerId = dealerId;
            ViewBag.ShiftId = shiftId;
            ViewBag.BranchId = branchId;
            ViewBag.UserId = userId;

            ViewBag.DealerName = dealerId == 0 ? "..." : new DealerService(User.GetSchema()).Get(dealerId).Name;
            ViewBag.ShiftName = shiftId == 0 ? "..." : new ShiftService(User.GetSchema()).Get(shiftId).Name;
            ViewBag.BranchName = branchId == 0 ? "..." : new BranchService(User.GetSchema()).Get(branchId).Name;
            ViewBag.UserName = userId == 0 ? "..." : new UserService(User.GetSchema()).Get(userId).Name;

            var obList = reportService.InvoiceDetail(1, fromDate.Value, toDate.Value, dealerId, shiftId, branchId, userId , page , pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SalesDetailList", obList) : View(obList);
        }


        public ActionResult PurchasDetail(DateTime? fromDate, DateTime? toDate, long dealerId = 0, long shiftId = 0, long branchId = 0, long userId = 0, int page = 1, int pageSize = 100)
        {
            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);

            if (dealerId == 2)
                dealerId = 0;

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.DealerId = dealerId;
            ViewBag.ShiftId = shiftId;
            ViewBag.BranchId = branchId;
            ViewBag.UserId = userId;

            ViewBag.DealerName = dealerId == 0 ? "..." : new DealerService(User.GetSchema()).Get(dealerId).Name;
            ViewBag.ShiftName = shiftId == 0 ? "..." : new ShiftService(User.GetSchema()).Get(shiftId).Name;
            ViewBag.BranchName = branchId == 0 ? "..." : new BranchService(User.GetSchema()).Get(branchId).Name;
            ViewBag.UserName = userId == 0 ? "..." : new UserService(User.GetSchema()).Get(userId).Name;


            var obList = reportService.PurchesDetail(2, fromDate.Value, toDate.Value, dealerId, shiftId, branchId, userId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("PurchasDetailList", obList) : View(obList);
        }


        public ActionResult Customer(long dealerId = 0, int page = 1, int pageSize = 100)
        {
            ViewBag.DealerId = dealerId;
            ViewBag.DealerName = dealerId == 0 ? "..." : new DealerService(User.GetSchema()).Get(dealerId).Name;
            var obList = reportService.Customers(1, dealerId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("Customer", obList) : View(obList);
        }

        public ActionResult TotalInvoiceCustomer(DateTime? fromDate, DateTime? toDate, long dealerId = 0, int page = 1, int pageSize = 100)
        {

            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.DealerId = dealerId;
            ViewBag.DealerName = dealerId == 0 ? "..." : new DealerService(User.GetSchema()).Get(dealerId).Name;
            var obList = reportService.TotainvoiceCustomers(1,fromDate.Value, toDate.Value, dealerId,  page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("TotalInvoiceCustomer", obList) : View(obList);
        }


        public ActionResult SuplierSheetReport(DateTime? fromDate, DateTime? toDate, long dealerId = 0, int page = 1, int pageSize = 100)
        {

            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.DealerId = dealerId;
            ViewBag.DealerName = dealerId == 0 ? "..." : new DealerService(User.GetSchema()).Get(dealerId).Name;
            var obList = reportService.SuplierSheetReportList(2, fromDate.Value, toDate.Value, dealerId,2,4, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SuplierSheetReport", obList) : View(obList);
        }

         public ActionResult CustomerSheetReport(DateTime? fromDate, DateTime? toDate, long dealerId = 0, int page = 1, int pageSize = 100)
        {

            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.DealerId = dealerId;
            ViewBag.DealerName = dealerId == 0 ? "..." : new DealerService(User.GetSchema()).Get(dealerId).Name;
            var obList = reportService.SuplierSheetReportList(1, fromDate.Value, toDate.Value, dealerId,1,3, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("CustomerSheetReport", obList) : View(obList);
        }
        public ActionResult TotalSales()
        {
            return View();
        }

        public ActionResult UnpaidInvoice()
        {
            return View();
        }        
    }
}