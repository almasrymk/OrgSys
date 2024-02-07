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




                int counter = 1;

                foreach (var item in data.Result)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

                    table.AddCell(new PdfPCell(new Phrase(item.SafeName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.TypeName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    }); 
                  
                    table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("dd/MM/yyyy"), bodyFont)));

                   
                    table.AddCell(new PdfPCell(new Phrase(item.Code.ToString(), bodyFont)));
                   

                    table.AddCell(new PdfPCell(new Phrase(item.DealerName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                 

                    table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString(), bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.CurrencyName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    counter++;
                }

                document.Add(table);
                document.Close();
                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "SafeMovement.pdf");
            }
        }



        public IActionResult SafeMovementExcel(string FromDate = null, string ToDate = null,
            long DealerId = 0, long SafeId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            var data = new ReportResult<SafeStatmentData, SafeStatment>
            {
                PrintMode = false,

                Result = new FinancesReportService(User.GetSchema()).GetSafeStatment(fDate, tDate, DealerId, SafeId, ShiftId, BranchId, UserId, page, pageSize)
            };


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Safe Movement");

            string[] headers = { "#", "SafeName", "FinancialType", "Date", "MotionCode", "Dealer" , "Amount", "Currency" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data.Result)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.SafeName;
                worksheet.Cells[row, 3].Value = item.TypeName;
                worksheet.Cells[row, 4].Value = item.Date.ToString("dd/MM/yyyy");
              
                worksheet.Cells[row, 5].Value = item.Code;
                worksheet.Cells[row, 6].Value = item.DealerName;
                worksheet.Cells[row, 7].Value = item.Amount;
                worksheet.Cells[row, 8].Value = item.CurrencyName;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SafeMovement.xlsx");
        }


        //public IActionResult SafeBalance(string ToDate = null,
        //   long SafeId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
        //   int page = 1, int pageSize = 100)
        //{
        //    ViewBag.SafeList = new SelectList(new SafeService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
        //    ViewBag.pageNumber = page;
        //    ViewBag.ParentId = 0;
        //    ViewBag.TypeId = 1;
        //    ViewBag.SafeId = SafeId;

        //    DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime tDate = DateTime.Now;

        //    if (ToDate != null)
        //        tDate = DateTime.Parse(ToDate);

        //    ViewBag.ToDate = tDate;

        //    var obList = new WarehousesReportService(User.GetSchema()).GetSafeBalance(1, tDate, SafeId, ShiftId, BranchId, UserId, page, pageSize);
        //    return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SafeBalanceList", obList) : View(obList);
        //}

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
