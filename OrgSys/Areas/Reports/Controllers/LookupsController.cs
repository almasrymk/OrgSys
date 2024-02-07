using Service;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
using System.IO;
using System;

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


        #region Products
        public ActionResult ProductsPdf(long ClassificationId, int page = 1, int pageSize = 900)
        {
            var data = new LookupsReportService(User.GetSchema()).GetProducts(ClassificationId, page, pageSize);

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
                var titleParagraph = new Paragraph("Products", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);
                PdfPTable table = new PdfPTable(new float[] { 1, 2, 1,2 })
                {
                    //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };


                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Name", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });
                table.AddCell(new PdfPCell(new Phrase("Barcode", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Classification", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                int counter = 1;
                foreach (var item in data)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.ItemName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    table.AddCell(new PdfPCell(new Phrase(item.BarCode.ToString(), bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.ClassificationName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    counter++;
                }
                document.Add(table);
                document.Close();

                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "Products.pdf");
            }
        }


        public IActionResult ProductsExcel(long ClassificationId, int page = 1, int pageSize = 900)
        {
            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
         
            var data = new LookupsReportService(User.GetSchema()).GetProducts(ClassificationId, page, pageSize);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");

            string[] headers = { "#", "Name", "Barcode" , "Classification" };


            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.ItemName;
                worksheet.Cells[row, 3].Value = item.BarCode;
                worksheet.Cells[row, 4].Value = item.ClassificationName;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
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


        #endregion

          #region Stocks
        public IActionResult Stocks(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;            
            var obList = new LookupsReportService(User.GetSchema()).GetStocks(search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StocksList", obList) : View(obList);
        }

        public ActionResult StocksPdf(string search, int page = 1, int pageSize = 900)
        {
            var data = new LookupsReportService(User.GetSchema()).GetStocks(search, page, pageSize);

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
                var titleParagraph = new Paragraph("Stocks", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);
                PdfPTable table = new PdfPTable(new float[] { 1, 6 })
                {
                    //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };


                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Name", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });
              
                int counter = 1;
                foreach (var item in data)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase(item.Name, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                
                   
                    counter++;
                }
                document.Add(table);
                document.Close();

                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "Stocks.pdf");
            }
        }


        public IActionResult StocksExcel(string search, int page = 1, int pageSize = 900)
        {
            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            var data = new LookupsReportService(User.GetSchema()).GetStocks(search, page, pageSize);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Stocks");

            string[] headers = { "#", "Name" };


            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.Name;
              
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Stocks.xlsx");
        }


        #endregion
        public IActionResult Safes(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            var obList = new LookupsReportService(User.GetSchema()).GetSafes(search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StocksList", obList) : View(obList);
        }
    }
}