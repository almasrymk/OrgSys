using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Model;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
using System.IO;

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


        public ActionResult SafeMovementPdf(string FromDate = null, string ToDate = null,
            long DealerId = 0, long SafeId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            var data = new ReportResult<SafeStatmentData, SafeStatment>
            {
                PrintMode = false,
              
                Result = new FinancesReportService(User.GetSchema()).GetSafeStatment(fDate, tDate, DealerId, SafeId, ShiftId, BranchId, UserId, page, pageSize)
            };

            using (var ms = new MemoryStream())
            {


                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, ms);
                document.Open();
                var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(50f, 50f);


                document.Add(logo);


                var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD);
                var headerFont = new Font(baseFont, 12, Font.BOLD);
                var bodyFont = new Font(baseFont, 12, Font.NORMAL);


                var titleParagraph = new Paragraph("Safe Movement", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);
                PdfPTable table = new PdfPTable(new float[] { 1, 2, 2, 2, 1,2,2,2 })
                {
                    //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };
                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("SafeName", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });

                table.AddCell(new PdfPCell(new Phrase("FinancialType", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Date", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("MotionCode", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Dealer", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Amount", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Currency", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });




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
