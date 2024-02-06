using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using OfficeOpenXml;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class SalesController : BaseReportController
    {
        //[Route("Reports/Sales/ExportToExcel")]

        public ActionResult ClientsBalancePdf(DateTime ToDate,
           long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 200)
        {
            var data = new SalesReportService(User.GetSchema()).GetDealersBalance(1, ToDate, DealerId, ShiftId, BranchId, UserId, page, pageSize); // Fetch your data

            using (var ms = new MemoryStream())
            {

              
                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, ms);
                document.Open();
                var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(50f, 50f); 
                
           
                document.Add(logo);
            
              
                var fontPath = Path.Combine("wwwroot", "font","cairo", "cairo-light.ttf");
               
                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD);
                var headerFont = new Font(baseFont, 12, Font.BOLD);
                var bodyFont = new Font(baseFont, 12, Font.NORMAL);

              
                var titleParagraph = new Paragraph("Clients Balance", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph); 
                PdfPTable table = new PdfPTable(new float[] { 1, 2, 1 })
                {
                 //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };
            
             
                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Dealer Name", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY ,});
                table.AddCell(new PdfPCell(new Phrase("Balance", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

                int counter = 1;
                foreach (var item in data)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.DealerName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT, 
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL 
                    });
                    table.AddCell(new PdfPCell(new Phrase(item.Balance.ToString(), bodyFont)));
                    counter++; 
                }

                document.Add(table);

                document.Close();

                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "ClientsBalance.pdf");
            }
        }

        public IActionResult ClientsBalanceExcel(DateTime ToDate ,
           long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 200)
        {
          


            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            // Fetching the data with the specified pageSize. Assuming GetDealersBalance can handle larger datasets efficiently

            var data = new SalesReportService(User.GetSchema()).GetDealersBalance(1, ToDate, DealerId, ShiftId, BranchId, UserId, page, pageSize);

            // Set up EPPlus for Excel generation
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Clients Balance");

            // Assuming 'data' is a list of some sort, let's define the headers
            string[] headers = { "#","Client", "Amount" }; // Adjust these headers based on your actual data structure

            // Adding headers to the Excel sheet
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            // Adding data to the Excel sheet
            int row = 2;
            int num = 1;// Starting from the second row, since the first row has headers
            foreach (var item in data) // Adjust this loop based on the structure of 'data'
            {
                worksheet.Cells[row, 1].Value = num; // Example, adjust to match your data structure
                worksheet.Cells[row, 2].Value = item.DealerName; // Example, adjust to match your data structure
                worksheet.Cells[row, 3].Value = item.Balance; // Example, adjust to match your data structure
               // Example, adjust as needed
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Converting the Excel package to a memory stream
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0; // Rewind the stream for reading

            // Return the Excel file
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientsBalance.xlsx");
        }

        public IActionResult ClientsStatment(string FromDate = null, string ToDate = null,
            long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 100)
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
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

            var obList = new ReportResult<DealerStatmentData,DealerStatment> { 
                PrintMode = false , 
                Data = new DealerStatmentData {  DealerBalance = new DealerBalance { DealerName = d.Name, DealerImgPath = d.ImgPath, DealerId = d.Id }, FromDate = fDate , ToDate = tDate },
                Result = new SalesReportService(User.GetSchema()).GetDealersStatment(1, fDate, tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize) 
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsStatmentList", obList) : View(obList);
        }

        public IActionResult ClientsBalance(string ToDate = null,
           long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize =100 )
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.ToDate = tDate;

            var obList = new SalesReportService(User.GetSchema()).GetDealersBalance(1 , tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsBalanceList", obList) : View(obList);
        }

        public IActionResult SalesPerPeriod()
        {
            return View();
        }

        public IActionResult SalesProductPerPeriod()
        {
            return View();
        }
    }
}