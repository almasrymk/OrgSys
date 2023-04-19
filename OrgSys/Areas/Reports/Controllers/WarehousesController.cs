using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class WarehousesController : Controller
    {
        public IActionResult StockMovement(string FromDate = null, string ToDate = null,
            long StockId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 100)
        {
            ViewBag.StockList = new SelectList(new StockService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.StockId = StockId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            
            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;

            var d = new StockService(User.GetSchema()).Get(StockId);
            if (d == null)
                d = new StockModelView();

            var obList = new ReportResult<StockStatmentData, StockStatment>
            {
                PrintMode = false,
                Data = new StockStatmentData { StockBalance = new StockBalance { StockName = d.Name, StockId = d.Id }, FromDate = fDate, ToDate = tDate },
                Result = new WarehousesReportService(User.GetSchema()).GetStockStatment(fDate, tDate, StockId, ShiftId, BranchId, UserId, page, pageSize)
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StockMovementList", obList) : View(obList);
        }

        public IActionResult ProductMovement(string FromDate = null, string ToDate = null,
            long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 100)
        {
            ViewBag.ProductList = new SelectList(new ProductService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.ProductId = ProductId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;

            var d = new ProductService(User.GetSchema()).Get(ProductId);
            if (d == null)
                d = new ProductModelView();

            var obList = new ReportResult<ProductStatmentData, ProductStatment>
            {
                PrintMode = false,
                Data = new ProductStatmentData { ProductBalance = new ProductBalance { ProductName = d.Name, ProductId = d.Id }, FromDate = fDate, ToDate = tDate },
                Result = new WarehousesReportService(User.GetSchema()).GetProductStatment(fDate, tDate, ProductId, ShiftId, BranchId, UserId, page, pageSize)
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ProductMovementList", obList) : View(obList);
        }

        public IActionResult StockBalance(string ToDate = null,
           long StockId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.StockList = new SelectList(new StockService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.ToDate = tDate;

            var obList = new WarehousesReportService(User.GetSchema()).GetStocksBalance(1, tDate, StockId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StockBalanceList", obList) : View(obList);
        }

        public IActionResult ProductBalance(string ToDate = null,
           long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.StockList = new SelectList(new StockService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            ViewBag.ToDate = tDate;

            var obList = new WarehousesReportService(User.GetSchema()).GetProductsBalance(1, tDate, ProductId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ProductBalanceList", obList) : View(obList);
        }

        public IActionResult TransaferReport()
        {
            return View();
        }

        public IActionResult ReceivedReport()
        {
            return View();
        }

        public IActionResult ProductStockInventoryList()
        {
            return View();
        }

        public IActionResult InventoryReport()
        {
            return View();
        }
        
    }
}
